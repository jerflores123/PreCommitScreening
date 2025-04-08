import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { UiCommonService } from '../ui-common/ui-common-service';
import { PreCommitScreeningClient, LookupsClient, PreCommitScreeningDto, PreCommitScreeningOutcomesDto, PreCommitScreeningParDto, PreCommitScreeningRecommendationsDto, PreCommitScreeningTypeDto, PrivilegeClient, GetCurrentUserFeaturePrivilegeResponse, CountyDto } from '../web-api-client';
import { PreCommitScreeningDetailsComponent } from './pre-commit-details/pre-commit-screening-details.component';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

declare const bootbox: any;

@Component({
  selector: 'app-pre-commit-screening',
  templateUrl: './pre-commit-screening.component.html',
  styleUrls: ['./pre-commit-screening.component.scss']
})
export class PreCommitScreeningComponent implements OnInit {
  _sin: string;
  items: PreCommitScreeningDto[] = [];
  screeningTypes: PreCommitScreeningTypeDto[] = [];
  counties: CountyDto[] = [];
  participents: PreCommitScreeningParDto[] = [];
  recommendations: PreCommitScreeningRecommendationsDto[] = [];
  Privilege: GetCurrentUserFeaturePrivilegeResponse;
  staffKey: number;

  constructor(private privilegeClient: PrivilegeClient, private uiService: UiCommonService, private route: ActivatedRoute, private preCommitScreeningClient: PreCommitScreeningClient, private lookupClient: LookupsClient, public dialog: MatDialog, authorizeService: AuthorizeService) {
    this.staffKey = authorizeService.currentUserValue.staffKey;
    this._sin = this.route.snapshot.paramMap.get('id');
    this.privilegeClient.getCurrentUserFeaturePrivilege('Pre Commit Screening').subscribe(
      result => {

        this.Privilege = result;
        if (this.Privilege.read_priv) {
          this.getScreeningTypes();
          this.getCounties();
          this.getScreeningParticipents();
          this.getScreeningRecommendations();
          this.getScreenings();
        }
      },
      error => { console.error(error) }
    );
  }

  ngOnInit(): void { }

  getScreenings() {
    this.preCommitScreeningClient.getScreenings(+this._sin).subscribe(res => {
      this.items = res.preCommitScreeningDto;
    });
  }

  getScreeningTypes() {
    let agencyid = Number(sessionStorage.getItem('agency_id'));
    this.preCommitScreeningClient.getScreeningTypes(agencyid).subscribe(res => {
      this.screeningTypes = res;
    });
  }

  getScreeningTypeById(type_id: number) {
    return this.screeningTypes?.find(f => f.screening_type_id === type_id)?.screening_type;
  }

  getCounties() {
    this.lookupClient.getCounties().subscribe(res => {
      this.counties = res;
    });
  }

  getScreeningParticipents() {
    this.preCommitScreeningClient.getScreeningParticipents().subscribe(res => {
      res.sort(c => c.orderBy);
      this.participents = res;
    });
  }

  getScreeningRecommendations() {
    let agencyid = Number(sessionStorage.getItem('agency_id'));
    this.preCommitScreeningClient.getScreeningRecommendations(agencyid).subscribe(res => {
      res.sort(c => c.orderBy);
      this.recommendations = res;
    });
  }

  OpenItem(item: PreCommitScreeningDto = null, cansaveedit = null) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.id = "contact-method-modal-component";
    dialogConfig.height = 'fit-content';
    dialogConfig.width = "700px";
    dialogConfig.data = {
      cansaveedit: cansaveedit,
      sin: this._sin,
      counties: this.counties,
      screeningTypes: this.screeningTypes,
      participents: this.participents,
      recommendations: this.recommendations,
      item
    };
    this.dialog.open(PreCommitScreeningDetailsComponent, dialogConfig).afterClosed().subscribe(res => {
      this.getScreenings();
    });
  }

  edit(row: PreCommitScreeningDto, cansaveedit) {
    this.OpenItem(row, cansaveedit);
  }

  delete(row: PreCommitScreeningDto): void {
    bootbox.confirm({
      message: "<i class='fa-solid fa fa-exclamation-triangle fa-3x float-left pr-3' style='color:red;'></i> Are you sure you want to remove selected Pre-Commit Screening? ",
      centerVertical: true,
      buttons: {
        confirm: {
          label: 'Yes',
          className: 'btn-primary'
        },
        cancel: {
          label: 'Cancel',
          className: 'btn-close mr-3'
        }
      },
      callback: (result) => {
        if (result) {
          this.preCommitScreeningClient.delete(row.screening_id).subscribe(res => {
            this.uiService.snackNotification('Record removed successfully!');
            this.getScreenings();
          });
        }
      }
    });
  }
}