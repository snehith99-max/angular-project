import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

interface IConfirmation {
  first_name: string;
  offer_gid: string;
  designation_name: string;
  department_name: string;
  branch_name: string;

  showUserPassword: boolean;
  showConfirmPassword: boolean;
  user_password: string;
  confirmpassword: string;
}

@Component({
  selector: 'app-hrm-mst-employeeconfirmation',
  templateUrl: './hrm-mst-employeeconfirmation.component.html',
  styleUrls: ['./hrm-mst-employeeconfirmation.component.scss']
})

export class HrmMstEmployeeconfirmationComponent {
  confirmationform!: FormGroup;
  confirmation!: IConfirmation;
  getconfirmationdetails: any;
  basic_salary1: any;
  template_name: any;
  responsedata: any;
  offergid: any;
  submitted = false;
  gross_salary: any;
  salarygradelist: any[] = [];
  Deduction_list: any[] = [];
  Others_list: any[] = [];
  salarygradetemplate_name: any;
  buttonClicked: boolean | undefined;
  salarygradetemplate_gid: any;
  Addition_list: any[] = [];
  calculate_deduction: any[] = [];
  mdljobtype: any;

  componentOptions: any[][] = [
    ['componentOptions1:any[] = []'],
    ['componentOptions2:any[] = []'],
    ['componentOptions3:any[] = []'],
    ['componentOptions4:any[] = []'],
    ['componentOptions5:any[] = []'],
    ['componentOptions6:any[] = []'],
  ];

  componentOptions1: any[][] = [
    ['componentOption1:any[] = []'],
    ['componentOption2:any[] = []'],
    ['componentOption3:any[] = []'],
    ['componentOption4:any[] = []'],
    ['componentOption5:any[] = []'],
    ['componentOption6:any[] = []'],
  ];

  calculate_addition: any[] = [];

  groupname: any[][] = [
    ['groupname1:any'],
    ['groupname2:any'],
    ['groupname3:any'],
    ['groupname4:any'],
    ['groupname5:any'],
    ['groupname6:any'],
  ];
  overallnetsalary: any;
  overallctcamount: any;

  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute,
    private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService) {
    this.confirmation = {} as IConfirmation;
  }

  ngOnInit(): void {

    this.confirmationform = new FormGroup({
      first_name: new FormControl(''),
      offer_gid: new FormControl(''),
      designation_name: new FormControl(''),
      department_name: new FormControl(''),
      branch_name: new FormControl(''),
      user_code: new FormControl('', Validators.required),
      user_status: new FormControl('Y'),
      jobtype: new FormControl(''),
      user_password: new FormControl('', Validators.required),
      confirmpassword: new FormControl(''),
      active_flag: new FormControl('Y'),
      template_name: new FormControl('', Validators.required),
      gross_salary: new FormControl('', Validators.required),
      salarygradetemplate_name: new FormControl('', Validators.required),

      employee_contribution_0: new FormControl(''),
      employee_contribution_1: new FormControl(''),
      employee_contribution_2: new FormControl(''),
      employee_contribution_3: new FormControl(''),
      employee_contribution_4: new FormControl(''),
      employee_contribution_5: new FormControl(''),
      employee_contribution_6: new FormControl(''),
      employee_contribution_7: new FormControl(''),
      employee_contribution_8: new FormControl(''),
      employee_contribution_9: new FormControl(''),

      employer_contribution_0: new FormControl(''),
      employer_contribution_1: new FormControl(''),
      employer_contribution_2: new FormControl(''),
      employer_contribution_3: new FormControl(''),
      employer_contribution_4: new FormControl(''),
      employer_contribution_5: new FormControl(''),
      employer_contribution_6: new FormControl(''),
      employer_contribution_7: new FormControl(''),
      employer_contribution_8: new FormControl(''),
      employer_contribution_9: new FormControl(''),

      demployee_contribution_0: new FormControl(''),
      demployee_contribution_1: new FormControl(''),
      demployee_contribution_2: new FormControl(''),
      demployee_contribution_3: new FormControl(''),
      demployee_contribution_4: new FormControl(''),
      demployee_contribution_5: new FormControl(''),

      demployer_contribution_0: new FormControl(''),
      demployer_contribution_1: new FormControl(''),
      demployer_contribution_2: new FormControl(''),
      demployer_contribution_3: new FormControl(''),
      demployer_contribution_4: new FormControl(''),
      demployer_contribution_5: new FormControl(''),

      componentgroup_gid: new FormControl(''),
      componentgroup_code: new FormControl(''),
      componentgroup_name: new FormControl(''),
      display_name: new FormControl(''),
      statutory: new FormControl(''),
      component_name: new FormControl(''),
      basic_salary: new FormControl(''),
      salarycomponent_name: new FormControl(''),
      salarygradetemplate_gid: new FormControl(''),
      employee_contribution: new FormControl(''),
      employer_contribution: new FormControl(''),
      demployee_contribution: new FormControl(''),
      demployer_contribution: new FormControl(''),
      grand_salary: new FormControl(''),
      ctc: new FormControl(''),
      net_salary: new FormControl(''),
      Addition_list: this.formBuilder.array([]),
      Deduction_list: this.formBuilder.array([]),
      Others_list: this.formBuilder.array([])

    });

    this.Getconfirmationdetails()


    var url = 'OfferLetter/Getgradetemplatedropdown';
    this.service.get(url).subscribe((result: any) => {
      this.salarygradelist = result.Getgradetemplatedropdown;
    });



  }

  Getconfirmationdetails() {
    const offer_gid = this.router.snapshot.paramMap.get('offer_gid');
    this.offergid = offer_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.offergid, secretKey).toString(enc.Utf8);

    let param = {
      offer_gid: deencryptedParam
    }

    var api = 'OfferLetter/Getconfirmationdetails';

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.getconfirmationdetails = result;
      this.confirmationform.get("offer_gid")?.setValue(this.getconfirmationdetails.offer_gid);
      this.confirmationform.get("first_name")?.setValue(this.getconfirmationdetails.first_name);
      this.confirmationform.get("designation_name")?.setValue(this.getconfirmationdetails.designation_name);
      this.confirmationform.get("department_name")?.setValue(this.getconfirmationdetails.department_name);
      this.confirmationform.get("branch_name")?.setValue(this.getconfirmationdetails.branch_name);
    });
  }


  onKeyPress(event: any) {
    // Get the pressed key
    const key = event.key;

    // Check if the key is a number or a dot (for decimal point)
    if (!/^[0-9.]$/.test(key)) {
      // If not a number or dot, prevent the default action (key input)
      event.preventDefault();
    }
  }


  fetchdetails1() {
    this.buttonClicked = !this.buttonClicked;

    this.getaddtionsummary();
    this.getdeductionsummary();
  }

  // otherssummary(){
  //   var url = 'OfferLetter/otherssummarybind'
  //   let param={

  //     salarygradetemplategid:this.confirmationform.value.template_name,
  //     gross_salary:this.confirmationform.value.gross_salary
  //   }
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     this.responsedata = result;
  //     this.Others_list = this.responsedata.othersSummarybind_list;

  //     this.confirmationform.get("basic_salary")?.setValue(this.Others_list[0].basicsalay);

  //     this.overallnetsalary = this.responsedata.netsalary;
  //     this.overallctcamount = this.responsedata.ctc

  //     this.confirmationform.get("net_salary")?.setValue(this.overallnetsalary);
  //     this.confirmationform.get("ctc")?.setValue(this.overallctcamount);
  //   });

  // }
  getaddtionsummary() {
    debugger;
    var url = 'OfferLetter/Additionsummarybind'
    let param = {

      salarygradetemplategid: this.confirmationform.value.template_name,
      gross_salary: this.confirmationform.value.gross_salary
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Addition_list = this.responsedata.Summarybind_list;

      this.confirmationform.get("basic_salary")?.setValue(this.Addition_list[0].basicsalay);


    });
  }
  getdeductionsummary() {
    debugger;
    var url = 'OfferLetter/deductionsummarybind'
    let param = {

      salarygradetemplategid: this.confirmationform.value.template_name,
      gross_salary: this.confirmationform.value.gross_salary
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Deduction_list = this.responsedata.deductionSummarybind_list;
      this.Others_list = this.responsedata.othersSummarybind_list;

      this.overallnetsalary = this.responsedata.overallnetsalary;
      this.overallctcamount = this.responsedata.ctc

      this.confirmationform.get("net_salary")?.setValue(this.overallnetsalary);
      this.confirmationform.get("ctc")?.setValue(this.overallctcamount);

    });
  }


  //  otherssummary(){
  //   var url = 'OfferLetter/otherssummarybind'
  //   let param={
  //     salarygradetemplate_gid:this.confirmationform.value.template_name,
  //     gross_salary:this.confirmationform.value.gross_salary
  //   }
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     this.responsedata = result;
  //     this.Addition_list = this.responsedata.Summarybind_list;
  //   });
  //  }

  formatCurrency(value: any) {
    const formattedValue = parseFloat(value).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    return formattedValue;
  }

  get user_password() {
    return this.confirmationform.get('user_password')!;
  }
  get confirmpassword() {
    return this.confirmationform.get('confirmpassword')!;
  }

  userpassword(user_password: any) {
    this.confirmationform.get("confirmpassword")?.setValue(user_password.value);
  }


  submit() {
    const offer_gid = this.router.snapshot.paramMap.get('offer_gid');
    this.offergid = offer_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.offergid, secretKey).toString(enc.Utf8);
    var params = {
      Summarybind_list: this.Addition_list,
      deductionSummarybind_list: this.Deduction_list,
      offer_gid: deencryptedParam,
      first_name: this.confirmationform.value.first_name,
      designation_name: this.confirmationform.value.designation_name,
      department_name: this.confirmationform.value.department_name,
      branch_name: this.confirmationform.value.branch_name,
      user_code: this.confirmationform.value.user_code,
      jobtype: this.confirmationform.value.jobtype,
      user_password: this.confirmationform.value.user_password,
      confirmpassword: this.confirmationform.value.confirmpassword,
      active_flag: this.confirmationform.value.active_flag,
      user_status: this.confirmationform.value.user_status,
      gross_salary: this.confirmationform.value.gross_salary,
      ctc: this.confirmationform.value.ctc,
      net_salary: this.confirmationform.value.net_salary,
      template_name: this.confirmationform.value.template_name,
      BasicSalary: this.confirmationform.value.basic_salary

    }
    console.log(params)

    var url = 'OfferLetter/Postemployeeconfirmation'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.route.navigate(['/hrm/HrmTrnOfferLetter']);
      }
      else {
        this.ToastrService.success(result.message)
        this.route.navigate(['/hrm/HrmTrnOfferLetter']);
      }
    });

  }
}
