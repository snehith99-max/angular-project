import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Location } from '@angular/common';


@Component({
  selector: 'app-pay-trn-payrunedit',
  templateUrl: './pay-trn-payrunedit.component.html',
  styleUrls: ['./pay-trn-payrunedit.component.scss']
})
export class PayTrnPayruneditComponent {
  payruneditform!: FormGroup;
  addition_list: any[] = [];
  deduction_list: any[] = [];
  others_list: any[] = [];
  salary_list:any[]=[];

  salary_gid_key: any;
  year_key: any;
  month_key: any;
  month:any;
  year:any;
  salary_gid : any;
  salary_gid_1 : any;
  responsedata: any;
  getpayrundetails: any;
  grossSalary: any;
  grand_salary: any;
  ctc: any;
  net_salary: any;
  basicsalary:any;
  showInputs: boolean = false;
  salary_mode:any;
  Basic:any;


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



  constructor(private renderer: Renderer2, private el: ElementRef,public NgxSpinnerService: NgxSpinnerService, private Location: Location,public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute) {
  }


  ngOnInit(): void{

    const salary_gid = this.router.snapshot.paramMap.get('salary_gid');
    this.salary_gid = salary_gid;
  
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salary_gid, secretKey).toString(enc.Utf8);

    this.salary_gid_1 = deencryptedParam;
    
  

    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);  
    
  
    this.Getpayrundetails();
    this.Getpayrunaddeditsummary();
    this.Getpayrundededitsummary();
    this.Getpayrunotherseditsummary();
    this.Getpayruneditsummary();
    // this.getbasicsalary();


    this.payruneditform = new FormGroup({
      employee_name : new FormControl(''),
      basic_salary: new FormControl(''),
      payrun_date : new FormControl('', [Validators.required]),
      addcomponentgroup_name: new FormControl(''),
      addsalarycomponent_name: new FormControl(''),
      addsalarycomponent_amount: new FormControl(''),
      addemployer_salarycomponentamount: new FormControl(''),
      gross_salary: new FormControl(''),
      grand_salary: new FormControl(''),
      dedcomponentgroup_name: new FormControl(''),
      dedsalarycomponent_name: new FormControl(''),
      dedsalarycomponent_amount: new FormControl(''),
      dedemployer_salarycomponentamount: new FormControl(''),
      otherscomponentgroup_name:new FormControl(''),
      otherssalarycomponent_name: new FormControl(''),
      otherssalarycomponent_amount:new FormControl(''),
      othersemployer_salarycomponentamount: new FormControl(''),
      net_salary: new FormControl(''),
      salary_gid: new FormControl(''),
      ctc: new FormControl(''),
      salary_mode:new FormControl(''),
    })

}

Getpayrundetails(){

  const salary_gid = this.router.snapshot.paramMap.get('salary_gid');
  this.salary_gid = salary_gid;

  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.salary_gid, secretKey).toString(enc.Utf8);
  
  let param = {
    salary_gid: deencryptedParam
  }

    var api = 'PayTrnSalaryManagement/Getpayrundetails';
debugger;
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata=result;
      this.getpayrundetails = result;
      this.payruneditform.get("salary_gid")?.setValue(this.getpayrundetails.salary_gid);
      this.payruneditform.get("employee_name")?.setValue(this.getpayrundetails.employee_name);
      this.payruneditform.get("basic_salary")?.setValue(this.getpayrundetails.basic_salary);
      this.payruneditform.get("payrun_date")?.setValue(this.getpayrundetails.payrun_date);
      this.payruneditform.get("salary_mode")?.setValue(this.getpayrundetails.salary_mode);

    });



}

// getbasicsalary(){
//   var api = 'PayTrnSalaryManagement/GetBasicSalary';
//   this.service.get(api).subscribe((result: any) => {
//     this.responsedata = result;
//     this.basicsalary = this.responsedata.basicsalarylist;
//   }); 
// }


Getpayrunaddeditsummary() {  
  
  const salary_gid = this.router.snapshot.paramMap.get('salary_gid');
  this.salary_gid = salary_gid;

  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.salary_gid, secretKey).toString(enc.Utf8);

  let param = {
    salary_gid: deencryptedParam,
    salarygradetype: "Addition"

  }

  debugger;  
  var url = 'PayTrnSalaryManagement/GetadditionEdit';
  
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata=result;
  this.addition_list = this.responsedata.Editaddition_list;

    setTimeout(() => {
      $('#addition_list').DataTable();
    },1);
    debugger;
    for (let i = 0; i < this.addition_list.length; i++) {

      this.payruneditform.addControl(`addcomponentgroup_name_${i}`, new FormControl(this.addition_list[i].addcomponentgroup_name));
      this.payruneditform.addControl(`addsalarycomponent_name_${i}`, new FormControl(this.addition_list[i].addsalarycomponent_name));
      this.payruneditform.addControl(`addsalarycomponent_amount_${i}`, new FormControl(this.addition_list[i].addsalarycomponent_amount));
      this.payruneditform.addControl(`addemployer_salarycomponentamount_${i}`, new FormControl(this.addition_list[i].addemployer_salarycomponentamount));

  
    }
    this.payruneditform.get("gross_salary")?.setValue(this.responsedata.gross_salary);
    this.payruneditform.get("grand_salary")?.setValue(this.responsedata.gross_salaryemployer);

  });
}


Getpayrundededitsummary(){
  debugger;

  const salary_gid = this.router.snapshot.paramMap.get('salary_gid');
  this.salary_gid = salary_gid;

  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.salary_gid, secretKey).toString(enc.Utf8);

  let param = {
    salary_gid: deencryptedParam,
    salarygradetype: "Deduction"

  }

  debugger;  
  var url = 'PayTrnSalaryManagement/GetdeductionEdit';
  
  this.service.getparams(url, param).subscribe((result: any) => {
  this.deduction_list = result.Editdeduction_list;

    setTimeout(() => {
      $('#deduction_list').DataTable();
    },1);
    debugger;
    for (let i = 0; i < this.deduction_list.length; i++) {

      this.payruneditform.addControl(`dedcomponentgroup_name_${i}`, new FormControl(this.deduction_list[i].dedcomponentgroup_name));
      this.payruneditform.addControl(`dedsalarycomponent_name_${i}`, new FormControl(this.deduction_list[i].dedsalarycomponent_name));
      this.payruneditform.addControl(`dedsalarycomponent_amount_${i}`, new FormControl(this.deduction_list[i].dedsalarycomponent_amount));
      this.payruneditform.addControl(`dedemployer_salarycomponentamount_${i}`, new FormControl(this.deduction_list[i].dedemployer_salarycomponentamount));

  
    }
  });

}


Getpayruneditsummary(){
  debugger;

  const salary_gid = this.router.snapshot.paramMap.get('salary_gid');
  this.salary_gid = salary_gid;

  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.salary_gid, secretKey).toString(enc.Utf8);

  let param = {
    salary_gid: deencryptedParam,

  }

  debugger;  
  var url = 'PayTrnSalaryManagement/GetSalaryEdit';
  
  this.service.getparams(url, param).subscribe((result: any) => {
  this.salary_list = result.Editsalary_list;
  this.payruneditform.get("net_salary")?.setValue(this.salary_list[0].net_salary);
  this.payruneditform.get("ctc")?.setValue(this.salary_list[0].ctc);
});

}

Getpayrunotherseditsummary(){

  const salary_gid = this.router.snapshot.paramMap.get('salary_gid');
  this.salary_gid = salary_gid;

  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.salary_gid, secretKey).toString(enc.Utf8);

  let param = {
    salary_gid: deencryptedParam,
    salarygradetype: "others"

  }

  debugger;  
  var url = 'PayTrnSalaryManagement/GetothersEdit';
  
  this.service.getparams(url, param).subscribe((result: any) => {
  this.others_list = result.Editothers_list;

    setTimeout(() => {
      $('#others_list').DataTable();
    },1);
    debugger;
    for (let i = 0; i < this.others_list.length; i++) {

      this.payruneditform.addControl(`otherscomponentgroup_name_${i}`, new FormControl(this.others_list[i].otherscomponentgroup_name));
      this.payruneditform.addControl(`otherssalarycomponent_name_${i}`, new FormControl(this.others_list[i].otherssalarycomponent_name));
      this.payruneditform.addControl(`otherssalarycomponent_amount_${i}`, new FormControl(this.others_list[i].otherssalarycomponent_amount));
      this.payruneditform.addControl(`othersemployer_salarycomponentamount_${i}`, new FormControl(this.others_list[i].othersemployer_salarycomponentamount));
    }
  });

}

onKeyPress(event: any) {
  // Get the pressed key
  const key = event.key;

  if (!/^[0-9.]$/.test(key)) {
      // If not a number or dot, prevent the default action (key input)
      event.preventDefault();
  }
}


get payrun_date() {
  return this.payruneditform.get('payrun_date')!;
}


ongrossSalaryChange(){
  debugger;

  let totalAddEmployeeContribution = 0;
  let totalAddEmployerContribution = 0;
  let totalDedEmployeeContribution = 0;
  let totalDedEmployerContribution = 0;
  let totalOthersEmployeeContribution = 0;
  let totalOthersEmployerContribution = 0;

  for (let i = 0; i < this.addition_list.length; i++) {
    totalAddEmployeeContribution += parseFloat(this.addition_list[i].addsalarycomponent_amount || 0);
  }

  for (let i = 0; i < this.addition_list.length; i++) {
    totalAddEmployerContribution += parseFloat(this.addition_list[i].addemployer_salarycomponentamount || 0);
  }

  for (let i = 0; i < this.deduction_list.length; i++) {
    totalDedEmployeeContribution += parseFloat(this.deduction_list[i].dedsalarycomponent_amount || 0);
  }

  for (let i = 0; i < this.deduction_list.length; i++) {
    totalDedEmployerContribution += parseFloat(this.deduction_list[i].dedemployer_salarycomponentamount || 0);
  }

  if (this.others_list && Array.isArray(this.others_list)) {
    for (let i = 0; i < this.others_list.length; i++) {
      totalOthersEmployeeContribution += parseFloat(this.others_list[i].otherssalarycomponent_amount || 0);
    }
  
    for (let i = 0; i < this.others_list.length; i++) {
      totalOthersEmployerContribution += parseFloat(this.others_list[i].othersemployer_salarycomponentamount || 0);
    }
  } else {
    console.warn('others_list is either null, undefined, or not an array.');
  }

  let totalAddEmployeeContribution1: number = Number(totalAddEmployeeContribution.toFixed(2));
  let totalAddEmployerContribution1: number = Number(totalAddEmployerContribution.toFixed(2));
  let totalDedEmployerContribution1: number = Number(totalDedEmployerContribution.toFixed(2));
  let totalDedEmployeeContribution1: number = Number(totalDedEmployeeContribution.toFixed(2));
  let totalOthersEmployeeContribution1: number = Number(totalOthersEmployeeContribution.toFixed(2));
  let totalOthersEmployerContribution1: number = Number(totalOthersEmployerContribution.toFixed(2));

  // Check if the form control exists and extract the basic_salary
  debugger;
  const basicSalaryControl = this.payruneditform.get('basic_salary');
  if (!basicSalaryControl) {
    console.error('basic_salary control not found');
    return;
  }
  let basic_salary: number = parseFloat(basicSalaryControl.value) || 0;
  console.log('Basic Salary:', basic_salary);

  // Update the calculations to include basic_salary
  let grossSalary: number = basic_salary + Number(totalAddEmployeeContribution1);
  let grand_salary: number = grossSalary + Number(totalAddEmployerContribution1);
  // let ctc: number = grand_salary + Number(totalDedEmployeeContribution1) + Number(totalDedEmployerContribution1);
  let net_salary: number = grossSalary - Number(totalDedEmployeeContribution1) - Number(totalOthersEmployeeContribution1);

  console.log('Gross Salary:', grossSalary);
  console.log('Grand Salary:', grand_salary);
  // console.log('CTC:', ctc);
  console.log('Net Salary:', net_salary);

  this.payruneditform.patchValue({
      gross_salary: grossSalary.toFixed(2),
      grand_salary: grand_salary.toFixed(2),
      net_salary: net_salary.toFixed(2),
      // ctc: ctc.toFixed(2),
  });

  this.grossSalary = grossSalary;
  this.grand_salary = grand_salary;
  // this.ctc = ctc;
  this.net_salary = net_salary;

  this.payruneditform.patchValue({
        gross_salary: this.grossSalary.toFixed(2),
        grand_salary: this.grand_salary.toFixed(2),
        net_salary: this.net_salary.toFixed(2),
        ctc: this.ctc.toFixed(2),
  });
}



update(){
  debugger;

  var params = {
    Editaddition_list: this.addition_list,
    Editdeduction_list: this.deduction_list,
    Editothers_list: this.others_list,
    Editsalary_list: this.salary_list,
    gross_salary: this.payruneditform.value.gross_salary,
    grand_salary: this.payruneditform.value.grand_salary,
    payrun_date: this.payruneditform.value.payrun_date,
    ctc: this.payruneditform.value.ctc,
    net_salary: this.payruneditform.value.net_salary,
    basic_salary: this.payruneditform.value.basic_salary,
    salary_gid: this.salary_gid_1
  }
     
  var url = 'PayTrnSalaryManagement/Updatepayrunedit';
  this.NgxSpinnerService.show();


  this.service.postparams(url, params).subscribe((result: any) => {
  this.NgxSpinnerService.hide();

    if (result.status == false) {
      this.ToastrService.warning(result.message)
      this.Location.back();
    }
    else {
      this.ToastrService.success(result.message)
      this.Location.back();
    }
  });
  
}

showTextBox(event: Event) {
    const target = event.target as HTMLInputElement;
  this.showInputs = target.value === 'Basic';
}

close(){
  this.Location.back();
}

}
