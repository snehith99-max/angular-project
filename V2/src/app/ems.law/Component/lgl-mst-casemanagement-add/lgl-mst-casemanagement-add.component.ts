import { Component } from '@angular/core';
import { FormControl, FormGroup ,Validators} from '@angular/forms';
import { dE } from '@fullcalendar/core/internal-common';
import flatpickr from 'flatpickr';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

interface ICase {
  case_date: Date;
  institute_name: string;
  case_no: string;
  client_name: string;
  casetype_name: string;
  case_remarks: string;
}


@Component({
  selector: 'app-lgl-mst-casemanagement-add',
  templateUrl: './lgl-mst-casemanagement-add.component.html',
  styleUrls: ['./lgl-mst-casemanagement-add.component.scss']
})
export class LglMstCasemanagementAddComponent {
  caseaddform!: FormGroup;
  caseInstitute_list: any[] = [];
  casetype_list: any[] = [];
  MdlCaseType: any;
  Mdlinstitiute: any;
  Mdlcasedate: any;
  case!: ICase;
  formData = new FormData();
  response_date: any;
  institutename:any;
  casetypename:any;
  casedate:any;

  constructor(public service: SocketService,
    private ToastrService: ToastrService, 
     private route: Router
  ) {
    this.case = {} as ICase;
  }

  ngOnInit(): void {
    debugger

    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);

    this.caseaddform = new FormGroup(
      {
        institutename:new FormControl(null, Validators.required),
        casetypename:new FormControl(null, Validators.required),
        casedate: new FormControl(null, Validators.required),
        case_no: new FormControl(''),
        client_name: new FormControl(''),
        case_remarks: new FormControl(''),
      });

    var api = 'CaseManagement/GetCasetype';
    this.service.get(api).subscribe((result: any) => {
      this.casetype_list = result.Getcasetype_list;
    });

    var api1 = 'CaseManagement/GetCaseInstitute';
    this.service.get(api1).subscribe((result: any) => {
      this.caseInstitute_list = result.Getcaseinstitute_list;
    });
  }


  onSubmit() {
    debugger
    var params = {
      casetype_name: this.caseaddform.value.casetypename.casetype_gid,
      institute_name: this.caseaddform.value.institutename.institute_gid,
      case_date: this.caseaddform.value.casedate,
      case_no :this.caseaddform.value.case_no,
      client_name :this.caseaddform.value.client_name,
      case_remarks :this.caseaddform.value.case_remarks,
    }
    var submitapi = 'CaseManagement/PostCaseInformation';
    this.service.post(submitapi,params).subscribe((result : any) =>{
      this.response_date = result;
      if (result.status == false) {
        
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({
          top: 0, 
        });

        this.route.navigate(['/legal/CaseManagement']);
        this.ToastrService.success(result.message)
      }
    });
  }
}
