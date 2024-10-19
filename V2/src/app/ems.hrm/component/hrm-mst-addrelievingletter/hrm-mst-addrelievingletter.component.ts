import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { AngularEditorConfig } from '@kolkov/angular-editor';

interface Iofferletter {
  Relievinglettertemplate_content: String;
}

@Component({
  selector: 'app-hrm-mst-addrelievingletter',
  templateUrl: './hrm-mst-addrelievingletter.component.html',
  styleUrls: ['./hrm-mst-addrelievingletter.component.scss']
})

export class HrmMstAddrelievingletterComponent {

  RelievingLetter: FormGroup | any;
  responsedata: any;
  Employee_list: any[] = [];
  Employeenamelist: any[] = [];
  salarywages: number = 0;
  ESIC: number = 0;
  Leave_Salary: number = 0;
  EPF: number = 0;
  Bonus: number = 0;
  LWF: number = 0;
  Gratuity: number = 0;
  Loan: number = 0;
  MainTotal: number = 0;
  AddTotal: number = 0;
  Total: number = 0;
  mdlTerms: any;
  Letter_list: any;
  templatecontent_list: any[] = [];
  Employee_data: any[] = [];
    mdlempcode: any;
  gender:any;

  emmployeename:any;
  idnumber:any;
  dateofjoin:any;
  depatment1:any;
  dateofrelieving:any;
  designation:any;
  services:any;  

  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '12rem',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };

  constructor(private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, private route: Router, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {

  }

  oncalculate() {
    this.Total = (+this.salarywages) + (+this.Leave_Salary) + (+this.Bonus) + (+this.Gratuity) - (+this.LWF) - (+this.ESIC) - (+this.EPF) - (+this.Loan)
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);

    this.RelievingLetter = new FormGroup({
      Total_Services: new FormControl(''),
      EmployeeCode: new FormControl(''),
      Name: new FormControl(''),
      Settlement_Date: new FormControl(''),
      ID_No: new FormControl(''),
      Date_of_Joining: new FormControl(''),
      Department: new FormControl(''),
      Date_of_Relieving: new FormControl(''),
      Designation: new FormControl(''),
      Reason_for_Settlement: new FormControl(''),
      Min_Wages: new FormControl(''),
      Salary: new FormControl(''),
      ESIC: new FormControl(''),
      Leave_Salary: new FormControl(''),
      EPF: new FormControl(''),
      Bonus: new FormControl(''),
      LWF: new FormControl(''),
      Gratuity: new FormControl(''),
      Loan: new FormControl(''),
      Total: new FormControl(''),
      employee_gid: new FormControl('', [Validators.required]),

      template_name: new FormControl('',[Validators.required]),
      Relievinglettertemplate_content: new FormControl(''),
    });

    var api = 'AddRelievingLetter/GetEmployeedropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.Employee_list = this.responsedata.Employeelists;
    });

    var url = 'AddRelievingLetter/RelivingLetterTemplate'
    this.service.get(url).subscribe((result: any) => {
      this.Letter_list = result.GetRelievingLetterdropdown;
    });
  }
  onClearemployee() {
    this.emmployeename = '';
    this.idnumber = '';
    this.dateofjoin='';
    this.depatment1 = '';
    this.dateofrelieving = '';
    this.designation='';
    this.services='';
   
  }

  employeeDetailsFetch() {
    debugger
    let Employeegid = this.RelievingLetter.get('employee_gid')?.value;
       

    let param = {
      Employeegid: Employeegid
    }
    this.NgxSpinnerService.show()
    var url = 'AddRelievingLetter/GetOnChangeEmployee';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Employeenamelist = this.responsedata.GetEmployeeonchangedetails;
      this.NgxSpinnerService.hide()
      this.RelievingLetter.get("Name")?.setValue(this.Employeenamelist[0].Name);
      this.RelievingLetter.get("ID_No")?.setValue(this.Employeenamelist[0].IDNo);
      this.RelievingLetter.get("Department")?.setValue(this.Employeenamelist[0].Department);
      this.RelievingLetter.get("Designation")?.setValue(this.Employeenamelist[0].Designation);
      this.RelievingLetter.get("Total_Services")?.setValue(this.Employeenamelist[0].TotalServices);
      this.RelievingLetter.get("Date_of_Joining")?.setValue(this.Employeenamelist[0].joiningdate);
      this.RelievingLetter.get("Date_of_Relieving")?.setValue(this.Employeenamelist[0].exit_date);
      this.NgxSpinnerService.hide()

    });
  }

  GetOnChangeTerms() {
    // Fetching the template_gid value from the form control
    let templategid = this.RelievingLetter.value.template_name;
    let param = {
        template_gid: templategid
    };

    // API call to fetch template content based on template_gid
    var url = 'AppointmentOrder/GetOnChangeTerms';
    this.service.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.templatecontent_list = this.responsedata.GetAppointmentdropdown;
        this.RelievingLetter.get("Relievinglettertemplate_content")?.setValue(this.templatecontent_list[0].template_content);

        // Fetching the Name value from form controls
        const user_name = this.RelievingLetter.value.Name;
        const userdesignation = this.RelievingLetter.value.Designation;
        const userJoiningDate = this.RelievingLetter.value.Date_of_Joining;  
        const existdate = this.RelievingLetter.value.Date_of_Relieving;   
 
        // const current = new Date();
        // const day1 = current.getDate();
        // const month1 = current.getMonth() + 1; // Months are zero-based in JS
        // const year1 = current.getFullYear();
        // const formattedCurrent = `${day1}-${month1}-${year1}`;  
        // const currentDate = new Date();
        // const day = currentDate.getDate();
        // const month = currentDate.getMonth() + 1; // Months are zero-based in JS
        // const year = currentDate.getFullYear();
        // const formattedCurrentDate = `${day}-${month}-${year}`;        
 

        let editContent = this.templatecontent_list[0].template_content;
        editContent = editContent.replace("username", user_name);
        editContent = editContent.replace("Senior Software Engineer", userdesignation);
        editContent = editContent.replace("Dateofjoining", userJoiningDate);        
        editContent = editContent.replace("Currentdate", existdate);

        // Update the form control with the edited content
        this.RelievingLetter.get("Relievinglettertemplate_content")?.setValue(editContent);
    });
}





  onsubmit() {
    this.NgxSpinnerService.show();
    var url = 'AddRelievingLetter/PostRelievingLetter';
    this.SocketService.post(url, this.RelievingLetter.value).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.route.navigate(['hrm/HrmMstReleivingLetter'])
        this.ToastrService.success(result.message);
        this.RelievingLetter.reset();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
        this.RelievingLetter.reset();
      }
      this.ngOnInit();
    })
  }

  back() {
    this.route.navigate(['hrm/HrmMstReleivingLetter']);
  }
}