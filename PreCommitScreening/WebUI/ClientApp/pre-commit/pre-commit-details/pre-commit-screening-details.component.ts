import { Component, OnInit, Inject } from "@angular/core";
import { FormArray, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ActivatedRoute } from "@angular/router";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { UiCommonService } from "src/app/ui-common/ui-common-service";
import { PreCommitScreeningClient, AssessmentQuestionsClient, PreCommitScreeningDto, PreCommitScreeningParDto, PreCommitScreeningRecommendationsDto, PreCommitScreeningTypeDto, PreCommitScreeningOutcomesDto, PreCommitScreeningVm, CreatePreCommitScreeningCommand, CountyDto } from "src/app/web-api-client";

export interface DialogData {
  cansaveedit: boolean;
  sin: number;
  item: PreCommitScreeningDto;
  outcomeItems: PreCommitScreeningOutcomesDto[];
  screeningTypes: PreCommitScreeningTypeDto[];
  counties: CountyDto[];
  participents: PreCommitScreeningParDto[];
  recommendations: PreCommitScreeningRecommendationsDto[];
}

@Component({
  selector: 'app-pre-commit-screening-details',
  templateUrl: './pre-commit-screening-details.component.html',
  styleUrls: ['./pre-commit-screening-details.component.scss']
})
export class PreCommitScreeningDetailsComponent implements OnInit {
  today = new Date();
  currentUser: any = "";
  formGroup: FormGroup;


  constructor(
    private preCommitScreeningClient: PreCommitScreeningClient,
    private authorizeService: AuthorizeService,
    private uiService: UiCommonService,
    public dialogRef: MatDialogRef<PreCommitScreeningDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {

    let recommended_screenings = this.data.outcomeItems?.filter(outcomes => outcomes.screening_rec_id).map(outcomes => outcomes.screening_rec_id) || [];
    let screening_participation = this.data.outcomeItems?.filter(outcomes => outcomes.screening_participant_id).map(outcomes => outcomes.screening_participant_id) || [];

    if (this.data.cansaveedit == true) {
      recommended_screenings = this.data.item.outcomes?.filter(outcomes => outcomes.screening_rec_id).map(outcomes => outcomes.screening_rec_id) || [];
      screening_participation = this.data.item.outcomes?.filter(outcomes => outcomes.screening_participant_id).map(outcomes => outcomes.screening_participant_id) || [];
    }
    this.formGroup = new FormGroup({
      'screening_type_id': new FormControl(this.data.item?.screening_type_id, [Validators.required]),
      'screening_date': new FormControl(this.data.item?.screening_date, [Validators.required]),
      'county_name': new FormControl(this.data.item?.county_name, [Validators.required]),
      'idjc_staff_present': new FormControl(this.data.item?.idjc_staff_present, [Validators.required]),
      'notes': new FormControl(this.data.item?.notes, [Validators.required]),
      'recommended_screenings': new FormArray(
        this.data.recommendations.map(recommendation => {
          return new FormGroup({
            ['recommendation_' + recommendation.screeningRecId]: new FormControl(recommended_screenings.some(r => r === recommendation.screeningRecId))
          })
        }),
        Validators.required
      ),
      'screening_participation': new FormArray(
        this.data.participents.map(participent => {
          return new FormGroup({
            ['participent_' + participent.screeningParticipantId]: new FormControl(screening_participation.some(r => r === participent.screeningParticipantId))
          })
        }),
        Validators.required
      )
    });
  }

  ngOnInit(): void {
    this.authorizeService.currentUser.subscribe(res => {
      this.currentUser = res?.userId;
    });
  }

  getFormValue(controlName: string) {
    return this.formGroup.get(controlName).value;
  }

  submit() {
    this.formGroup.markAllAsTouched();
    if (this.formGroup.valid) {

      let preCommitScreeningOutcomesDtos: PreCommitScreeningOutcomesDto[] = [];
      const recommended_screenings = this.formGroup.get('recommended_screenings').value;
      const screening_participation = this.formGroup.get('screening_participation').value;

      const rec_keys = recommended_screenings.filter(rec => rec[Object.keys(rec)[0]]).map(rec => Object.keys(rec)[0].replace("recommendation_", ""));
      const par_keys = screening_participation.filter(rec => rec[Object.keys(rec)[0]]).map(rec => Object.keys(rec)[0].replace("participent_", ""));

      rec_keys.forEach(row => {
        preCommitScreeningOutcomesDtos.push(
          PreCommitScreeningOutcomesDto.fromJS({
            screening_rec_id: +row,
            created_by: this.currentUser
          })
        );
      });

      par_keys.forEach(row => {
        preCommitScreeningOutcomesDtos.push(
          PreCommitScreeningOutcomesDto.fromJS({
            screening_participant_id: +row,
            created_by: this.currentUser
          })
        );
      });

      let preCommitScreeningDto = PreCommitScreeningDto.fromJS({
        sin: +this.data.sin,
        screening_date: this.getFormValue("screening_date"),
        county_name: this.getFormValue("county_name"),
        idjc_staff_present: this.getFormValue("idjc_staff_present").toString(),
        notes: this.getFormValue("notes"),
        screening_type_id: +this.getFormValue("screening_type_id"),
        created_by: this.currentUser,
        modified_by: this.currentUser,
        outcomes: preCommitScreeningOutcomesDtos
      });

      if (this.data.item) {
        preCommitScreeningDto.screening_id = this.data.item.screening_id;
      }

      let screeningVm = PreCommitScreeningVm.fromJS({
        preCommitScreeningDto: [preCommitScreeningDto],
      });

      let postData = CreatePreCommitScreeningCommand.fromJS({ screeningVm: screeningVm });
      if (this.data.item) {
        this.preCommitScreeningClient.update(postData).subscribe(res => {
          this.uiService.snackNotification('Pre-Commit Screening Updated successfully!');
          this.dialogRef.close(true);
        });
      } else {
        this.preCommitScreeningClient.create(postData).subscribe(res => {
          this.uiService.snackNotification('Pre-Commit Screening Created successfully!');
          this.dialogRef.close(true);
        });
      }
    }
  }

}
