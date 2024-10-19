import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';

interface Iexperienceletter {
  employee_gid: string;
  employee_name: string;
  Experiencelettertemplate_content: String;
  offertemplate_content: string;
  joiningdate: string;
}

interface Employee {
  employee_name: string;
  employee_joiningdate: string;
  exit_date: string;
}

@Component({
  selector: 'app-hrm-mst-experienceletterdirectadd',
  templateUrl: './hrm-mst-experienceletterdirectadd.component.html',
  styleUrls: ['./hrm-mst-experienceletterdirectadd.component.scss']
})

export class HrmMstExperienceletterdirectaddComponent {
  employee_list: any[] = [];
  employee_gid: any;
  appointmentorder!: Iexperienceletter;
  responsedata: any;
  templatecont_list: any[] = [];
  term_list: any[] = [];
  employeedata: any[] = [];
  mdlTerms: any;
  mdlempname: any;
  template_gid: any;
  appointmentordergid: any;
  Experienceletterform!: FormGroup;
  editContent: string = "";

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

  constructor(private renderer: Renderer2, private el: ElementRef, public NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute) {
    this.appointmentorder = {} as Iexperienceletter;

    this.Experienceletterform = new FormGroup({
      appointmentorder_gid: new FormControl(''),
      employee_name: new FormControl('', Validators.required),
      employee_gid: new FormControl(''),
      template_gid: new FormControl(''),
      template_name: new FormControl('',[Validators.required]),
      Experiencelettertemplate_content: new FormControl(''),
    });
  }

  ngOnInit(): void {
    var api = 'HrmMstExperienceLetter/GetUserDetail'
    this.service.get(api).subscribe((result: any) => {
      this.employee_list = result.GetUserDtl;      
    });

    var url = 'AppointmentOrder/TermsandConditions'
    this.service.get(url).subscribe((result: any) => {
      this.term_list = result.GetAppointmentdropdown;
    });
  }

  GetOnChangeEmployee(event: Event) {
    debugger;
    this.employee_gid = this.Experienceletterform.value.employee_name;
    let param = {
      employee_gid: this.employee_gid
    }

    var url = 'HrmMstExperienceLetter/GetOnChangeEmployee';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.employeedata = this.responsedata.GetEmployeeList;
    });

    const selectedEmployeeName = (event.target as HTMLSelectElement).value;

    // Find the selected employee
    const employees: Employee[] = [];
    const selectedEmployee = employees.find(employee => employee.employee_name === selectedEmployeeName);

    if (selectedEmployee) {
      // Display the joining date and exit date
      console.log(`Joining Date: ${selectedEmployee.employee_joiningdate.toString()}`);
      console.log(`Exit Date: ${selectedEmployee.exit_date ? selectedEmployee.exit_date.toString() : 'Still Employed'}`);
    } else {
      console.log("Employee not found!");
    }
  }

  GetOnChangeTerms() {

    this.template_gid = this.Experienceletterform.value.template_name;
    let param = {
      template_gid: this.template_gid
    }

    var url = 'AppointmentOrder/GetOnChangeTerms';

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.templatecont_list = this.responsedata.GetAppointmentdropdown;
      this.Experienceletterform.get("Experiencelettertemplate_content")?.setValue(this.templatecont_list[0].template_content);

      // Fetching values from form controls
      const userFirstName = this.Experienceletterform.value.employee_name;
      const userJoiningDate = this.Experienceletterform.value.joiningdate;
      const userexitdate = this.Experienceletterform.value.exit_date;
      
      // Replacing placeholders with actual values
      let editContent = this.templatecont_list[0].template_content;
      editContent = editContent.replace("first_name", this.employeedata[0].employee_name + ' ' + this.employeedata[0].user_lastname + ' / ' + this.employeedata[0].user_code);
      editContent = editContent.replace("Joiningdate", this.employeedata[0].employee_joiningdate);
      editContent = editContent.replace("Relieving date", this.employeedata[0].exit_date);
      editContent = editContent.replace("firstname", this.employeedata[0].employee_name + ' ' + this.employeedata[0].user_lastname);
      editContent = editContent.replace("Designation", this.employeedata[0].designation_name);

      // Setting the updated content in the form control
      this.Experienceletterform.get("Experiencelettertemplate_content")?.setValue(editContent);
    });
  }

  onsubmit() {
    debugger;
    var params = {
      employee_gid: this.Experienceletterform.value.employee_name,
      template_gid: this.Experienceletterform.value.template_name,
      Experiencelettertemplate_content: this.Experienceletterform.value.Experiencelettertemplate_content,
      first_name: this.Experienceletterform.value.employee_name,
      lsjoindate: this.Experienceletterform.value.joiningdate,
    }

    var url = 'HrmMstExperienceLetter/Addexperienceletter'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
        this.route.navigate(['/hrm/HrmMstExperienceletter']);
      }
    });
  }

  onback() {
    this.route.navigate(['/hrm/HrmMstExperienceletter']);
  }
  
  get employee_name() {
    return this.Experienceletterform.get('employee_name')!;
  }
  get template_name() {
    return this.Experienceletterform.get('template_name')!;
  }
}
