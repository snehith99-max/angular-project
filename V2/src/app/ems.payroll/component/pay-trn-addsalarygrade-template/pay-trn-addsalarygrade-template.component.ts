import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { HttpParams } from '@angular/common/http';
import { AES } from 'crypto-js';
import { Options } from 'flatpickr/dist/types/options';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';
import { data } from 'jquery';
interface ISalaryGrade {
  componentgroup_gid: string;
  componentgroup_code: string;
  componentgroup_name: string;

  display_name: string;
  statutory: string;
  net_salary: string;
  template_name: string,
}

@Component({
  selector: 'app-pay-trn-addsalarygrade-template',
  templateUrl: './pay-trn-addsalarygrade-template.component.html',
  styleUrls: ['./pay-trn-addsalarygrade-template.component.scss']
})
export class PayTrnAddsalarygradeTemplateComponent {
  reactiveFormadd!: FormGroup;
  submitted = false;
  SalaryGrade!: ISalaryGrade;
  group_name: any;
  grossSalary: any;
  buttonClicked: boolean | undefined;
  grand_salary: any;
  ctc: any;
  net_salary: any;
  gross_salary: any;
  basic_salary: any;
  template_name: any;
  componentnames: any[] = [];
  component_name: any[] = [];
  addition_list: any[] = [];
  deduction_list: any[] = [];
  others_list: any[] = [];
  calculate_addition: any[] = []
  calculate_deduction: any[] = []
  calculate_others: any[] = []
  componentOptionsadd: any[][] = [
    ['componentOptions1:any[] = []'],
    ['componentOptions2:any[] = []'],
    ['componentOptions3:any[] = []'],
    ['componentOptions4:any[] = []'],
    ['componentOptions5:any[] = []'],
    ['componentOptions6:any[] = []'],
  ];
  componentOptionsded: any[][] = [
    ['componentOption1:any[] = []'],
    ['componentOption2:any[] = []'],
    ['componentOption3:any[] = []'],
    ['componentOption4:any[] = []'],
    ['componentOption5:any[] = []'],
    ['componentOption6:any[] = []'],
  ];
  componentOptionsothers: any[][] = [
    ['componentOption1:any[] = []'],
    ['componentOption2:any[] = []'],
    ['componentOption3:any[] = []'],
    ['componentOption4:any[] = []'],
    ['componentOption5:any[] = []'],
    ['componentOption6:any[] = []'],
  ];
  responsedata: any;
  selection: any;
  groupname: any[][] = [
    [''],
    [''],
    [''],
  ];
  group: any[][] = [
    [''],
    [''],
    [''],
    [''],
    [''],
  ];
  groupothers: any[][] = [
    [''],
    [''],
    [''],
    [''],
    [''],
  ];

  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    public service: SocketService,
    private router: Router,
    public NgxSpinnerService: NgxSpinnerService,) {

  }
  ngOnInit(): void {
    // this.Addtictionsummary();
    // this.deductionsummary();
    this.reactiveFormadd = new FormGroup({
      componentgroup_gid: new FormControl(''),
      componentgroup_code: new FormControl(''),
      componentgroup_name: new FormControl(''),
      display_name: new FormControl(''),
      statutory: new FormControl(''),
      template_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      component_name: new FormControl(''),
      basic_salary: new FormControl('',Validators.required),
      employee_contribution: new FormControl(''),
      employer_contribution: new FormControl(''),
      demployee_contribution: new FormControl(''),
      demployer_contribution: new FormControl(''),
      oemployee_contribution: new FormControl(''),
      oemployer_contribution: new FormControl(''),
      gross_salary: new FormControl(''),
      grand_salary: new FormControl(''),
      ctc: new FormControl(''),
      net_salary: new FormControl(''),
      salary_name: new FormControl(''),
      addition_list: this.formBuilder.array([]),
      deduction_list: this.formBuilder.array([]),
      others_list: this.formBuilder.array([])
    });
  }

  onBasicSalaryChange() {
    debugger

    let totalAddEmployeeContribution = 0;
    let totalAddEmployerContribution = 0;
    let totalDedEmployeeContribution = 0;
    let totalDedEmployerContribution = 0;
    let totalOthersEmployeeContribution = 0;
    let totalOthersEmployerContribution = 0;


    for (let i = 0; i < this.addition_list.length; i++) {
      totalAddEmployeeContribution += parseFloat(this.addition_list[i].employee_contribution || 0);
    }

    for (let i = 0; i < this.addition_list.length; i++) {
      totalAddEmployerContribution += parseFloat(this.addition_list[i].employer_contribution || 0);
    }

    debugger;
    for (let i = 0; i < this.deduction_list.length; i++) {
      totalDedEmployeeContribution += parseFloat(this.deduction_list[i].demployee_contribution || 0);
    }

    for (let i = 0; i < this.deduction_list.length; i++) {
      totalDedEmployerContribution += parseFloat(this.deduction_list[i].demployer_contribution || 0);
    }

    if (this.others_list && Array.isArray(this.others_list)) {
      for (let i = 0; i < this.others_list.length; i++) {
         totalOthersEmployeeContribution += parseFloat(this.others_list[i].oemployee_contribution || 0);
       }
    
       for (let i = 0; i < this.others_list.length; i++) {
         totalOthersEmployerContribution += parseFloat(this.others_list[i].oemployer_contribution || 0);
       }
     } 
     else {
    }



    const newBasicSalary = parseFloat(this.reactiveFormadd.get('basic_salary')?.value || 0);

    let basic_salary1: number = Number(newBasicSalary.toFixed(2));
    let totalAddEmployeeContribution1: number = Number(totalAddEmployeeContribution.toFixed(2));
    let totalAddEmployerContribution1: number = Number(totalAddEmployerContribution.toFixed(2));
    let totalDedEmployerContribution1: number = Number(totalDedEmployerContribution.toFixed(2));
    let totalDedEmployeeContribution1: number = Number(totalDedEmployeeContribution.toFixed(2));
    let totalOthersEmployeeContribution1: number = Number(totalOthersEmployeeContribution.toFixed(2));
    let totalOthersEmployerContribution1: number = Number(totalOthersEmployeeContribution.toFixed(2));


    let grossSalary: number = Number(totalAddEmployeeContribution1);
    let grand_salary: number = Number(grossSalary) + Number(totalAddEmployerContribution1) + Number(basic_salary1);
    let gross_salary: number = Number(grossSalary) + Number(totalAddEmployerContribution1) + Number(basic_salary1);

    let ctc: number = Number(grand_salary) + Number(totalDedEmployerContribution1);
    let net_salary: number = Number(grossSalary) - Number(totalDedEmployeeContribution1) - Number(totalDedEmployerContribution1) - Number(totalOthersEmployeeContribution1);



    this.reactiveFormadd.patchValue({
      basic_salary: basic_salary1.toFixed(2),
      gross_salary: gross_salary.toFixed(2),
      grand_salary: grand_salary.toFixed(2),
      net_salary: net_salary.toFixed(2),
      ctc: ctc.toFixed(2),
    });


    debugger;
    const salarytype = this.reactiveFormadd.value.salary_name;
    if (salarytype == "GROSS") {
      this.grossSalary = Number(totalAddEmployeeContribution1);
      this.grand_salary = Number(totalAddEmployeeContribution1);
      this.ctc = Number(grand_salary) + Number(totalDedEmployeeContribution1) + Number(totalDedEmployerContribution1);
      this.net_salary = Number(totalAddEmployeeContribution1) - Number(totalDedEmployeeContribution1) - Number(totalDedEmployerContribution1) - Number(totalOthersEmployeeContribution1);

      this.reactiveFormadd.patchValue({
        gross_salary: this.grossSalary.toFixed(2),
        grand_salary: this.grand_salary.toFixed(2),
        net_salary: this.net_salary.toFixed(2),
        ctc: this.ctc.toFixed(2),
      });

    }
    else {
      this.grossSalary = Number(totalAddEmployeeContribution1);
      this.grand_salary = Number(grossSalary) + Number(totalAddEmployerContribution1) + Number(basic_salary1);
      this.gross_salary = Number(grossSalary) + Number(totalAddEmployerContribution1) + Number(basic_salary1);


      this.ctc = Number(grand_salary) + Number(totalDedEmployerContribution1);
      this.net_salary = Number(this.grand_salary) - Number(totalDedEmployeeContribution1);


      this.reactiveFormadd.patchValue({
        basic_salary: basic_salary1.toFixed(2),
        gross_salary: this.gross_salary.toFixed(2),
        grand_salary: this.grand_salary.toFixed(2),
        net_salary: this.net_salary.toFixed(2),
        ctc: this.ctc.toFixed(2),
      });
    }
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


  Addtictionsummary() {
    debugger;
    var url = 'PayTrnSalaryGrade/AdditionComponentSummary';

    let param = {
      salarytype: this.reactiveFormadd.value.salary_name,
      salarygradetemplate_name: this.reactiveFormadd.value.template_name,
      basic_salary: this.reactiveFormadd.value.basic_salary

    };


    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.addition_list = this.responsedata.addition_list;
      this.triggerGetOptions();
      // this.addition_list.forEach((item) => {
      //   item.employee_contribution = ('');
      //   item.employer_contribution = ('');
      // });

      setTimeout(() => {
        $('#addition_list').DataTable();
      }, 1);


    });

  }

  calculateaddition1(salarycomponent_gid: string, n: number): void {

    var url1 = 'PayTrnSalaryGrade/Getcomponentamount';
    let param: { salarycomponent_gid: any } = {
      salarycomponent_gid: salarycomponent_gid
    };
    this.service.getparams(url1, param).subscribe((result: any) => {

      this.calculate_addition[0] = result.Getcomponentamount[0];
      const component_flag_employer = this.calculate_addition[0].component_flag_employer;
      const component_flag = this.calculate_addition[0].component_flag;
      const component_percentage = this.calculate_addition[0].component_percentage;
      const component_amount = this.calculate_addition[0].component_amount;
      const component_percentage_employer = this.calculate_addition[0].component_percentage_employer;
      const component_amount_employer = this.calculate_addition[0].component_amount_employer;
      const newBasicSalary = this.reactiveFormadd.get('basic_salary')?.value;


      if (component_flag == "Y") {
        this.addition_list[n].employee_contribution = Number((newBasicSalary) * (component_percentage / 100)).toFixed(2)
        this.reactiveFormadd.addControl(`employee_contribution_${n}`, new FormControl(this.addition_list[n].employee_contribution));
      }
      else {
        this.addition_list[n].employee_contribution = Number(component_amount).toFixed(2)
        this.reactiveFormadd.addControl(`employee_contribution_${n}`, new FormControl(this.addition_list[n].employee_contribution));
      }

      if (component_flag_employer == "Y") {

        this.addition_list[n].employer_contribution = Number((newBasicSalary) * (component_percentage_employer / 100)).toFixed(2)
        this.reactiveFormadd.addControl(`employer_contribution_${n}`, new FormControl(this.addition_list[n].employer_contribution));
      } else {

        this.addition_list[n].employer_contribution = Number(component_amount_employer).toFixed(2)
        this.reactiveFormadd.addControl(`employer_contribution_${n}`, new FormControl(this.addition_list[n].employer_contribution));
      }

    });

  }

  getoptions(componentgroup_name: any, n: number) {
    var url1 = 'PayTrnSalaryGrade/Getcomponentname';
    let param: { componentgroup_name: any } = {
      componentgroup_name: componentgroup_name
    };
    this.service.getparams(url1, param).subscribe((result: any) => {
      this.componentOptionsadd[n] = result.GetComponentname;
    });
  }

  deductionsummary() {
    debugger;
    var url = 'PayTrnSalaryGrade/DeductionComponentSummary'

    let param = {
      salarytype: this.reactiveFormadd.value.salary_name,
      salarygradetemplate_name: this.reactiveFormadd.value.template_name,
      basic_salary: this.reactiveFormadd.value.basic_salary
    }

    this.service.getparams(url, param).subscribe((result: any) => {

      this.responsedata = result;
      this.deduction_list = this.responsedata.deduction_list;
      this.triggerGetOptions1();
      // this.deduction_list.forEach((item1 :any) => {
      //   item1.demployee_contribution = ('');
      //   item1.demployer_contribution = ('');
      // });


      setTimeout(() => {
        $('#deduction_list').DataTable();
      }, 1);


    });

  }

  Otherssummary() {
    debugger;
    var url = 'PayTrnSalaryGrade/OthersComponentSummary';

    let param = {
      salarytype: this.reactiveFormadd.value.salary_name,
      salarygradetemplate_name: this.reactiveFormadd.value.template_name,
      basic_salary: this.reactiveFormadd.value.basic_salary
    };

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.others_list = this.responsedata.others_list;
      this.triggerGetOptions2();
      this.others_list.forEach((item) => {
        item.oemployee_contribution = ('');
        item.oemployer_contribution = ('');
      });

      setTimeout(() => {
        $('#others_list').DataTable();
      }, 1);


    });

  }


  triggerGetOptions(): void {
    for (let i = 0; i < this.addition_list.length; i++) {
      const data = this.addition_list[i];
      this.getoptions(data.componentgroup_name, i);
    }
  }
  triggerGetOptions1(): void {
    debugger;
    for (let i = 0; i < this.deduction_list.length; i++) {
      const data = this.deduction_list[i];
      this.getdeduction(data.componentgroup_name, i);
    }
  }

  triggerGetOptions2(): void {
    for (let i = 0; i < this.others_list.length; i++) {
      const data = this.others_list[i];
      this.getotherscomponents(data.componentgroup_name, i);
    }
  }

  getdeduction(componentgroup_name: any, n: number) {
    debugger;
    var url1 = 'PayTrnSalaryGrade/Getcomponentname';
    let param: { componentgroup_name: any } = {
      componentgroup_name: componentgroup_name
    };

    const gross_salary = this.reactiveFormadd.value.gross_salary;
    const basic_salary = this.reactiveFormadd.value.basic_salary;
    if (componentgroup_name == "ESI" && gross_salary >= 21000) {
      this.reactiveFormadd.addControl(`demployer_contribution_${n}`, new FormControl(0));
      this.reactiveFormadd.addControl(`demployee_contribution_${n}`, new FormControl(0));
      this.componentOptionsded[n] = [];

    }

    else {
      this.service.getparams(url1, param).subscribe((result: any) => {
        this.componentOptionsded[n] = result.GetComponentname;


      });
    }
  }

  getotherscomponents(componentgroup_name: any, n: number) {
    var url1 = 'PayTrnSalaryGrade/Getcomponentname';
    let param: { componentgroup_name: any } = {
      componentgroup_name: componentgroup_name
    };

    this.service.getparams(url1, param).subscribe((result: any) => {
      this.componentOptionsothers[n] = result.GetComponentname;
    });

  }

  calculateothers(salarycomponent_gid: string, n: number): void {

    var url1 = 'PayTrnSalaryGrade/Getcomponentamount';
    let param: { salarycomponent_gid: any } = {
      salarycomponent_gid: salarycomponent_gid
    };
    this.service.getparams(url1, param).subscribe((result: any) => {

      this.calculate_others[0] = result.Getcomponentamount[0];
      const component_flag_employer = this.calculate_others[0].component_flag_employer;
      const component_flag = this.calculate_others[0].component_flag;
      const component_percentage = this.calculate_others[0].component_percentage;
      const component_amount = this.calculate_others[0].component_amount;
      const component_percentage_employer = this.calculate_others[0].component_percentage_employer;
      const component_amount_employer = this.calculate_others[0].component_amount_employer;
      const newBasicSalary = this.reactiveFormadd.get('basic_salary')?.value;


      if (component_flag == "Y") {
        this.others_list[n].oemployee_contribution = Number((newBasicSalary) * (component_percentage / 100)).toFixed(2)
        this.reactiveFormadd.addControl(`oemployee_contribution_${n}`, new FormControl(this.others_list[n].oemployee_contribution));
      }
      else {
        this.others_list[n].oemployee_contribution = Number(component_amount).toFixed(2)
        this.reactiveFormadd.addControl(`oemployee_contribution_${n}`, new FormControl(this.others_list[n].oemployee_contribution));
      }

      if (component_flag_employer == "Y") {

        this.others_list[n].oemployer_contribution = Number((newBasicSalary) * (component_percentage_employer / 100)).toFixed(2)
        this.reactiveFormadd.addControl(`oemployer_contribution_${n}`, new FormControl(this.others_list[n].oemployer_contribution));
      } else {

        this.others_list[n].oemployer_contribution = Number(component_amount_employer).toFixed(2)
        this.reactiveFormadd.addControl(`oemployer_contribution_${n}`, new FormControl(this.others_list[n].oemployer_contribution));
      }

    });

  }

  submit() {

    debugger;
    this.submitted = true;
    console.log(this.addition_list)
    console.log(this.deduction_list)


    // for(let i=0; i< this.groupname.length;i++){
    //   this.componentnames.push(this.groupname[i]);
    // }

    var params = {
      addition_list: this.addition_list,
      deduction_list: this.deduction_list,
      others_list: this.others_list,
      newBasicSalary: this.reactiveFormadd.value.basic_salary,
      template_name: this.reactiveFormadd.value.template_name,
      gross_salary: this.reactiveFormadd.value.gross_salary,
      ctc: this.reactiveFormadd.value.ctc,
      net_salary: this.reactiveFormadd.value.net_salary,
      component_name: this.groupname.flat(),
      component_name1: this.group.flat(),
      component_name2: this.groupothers.flat()
    }

    console.log(params)

    var url = 'PayTrnSalaryGrade/PostSalaryGrade'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.router.navigate(['/payroll/PayTrnSalaryGradeTemplate']);

      }
      else {
        this.ToastrService.success(result.message)
        this.router.navigate(['/payroll/PayTrnSalaryGradeTemplate']);


      }

    });

  }
  calculatededuction(salarycomponent_gid: string, n: number): void {

    debugger;
    var url1 = 'PayTrnSalaryGrade/Getcomponentamount';
    let param: { salarycomponent_gid: any } = {
      salarycomponent_gid: salarycomponent_gid
    };
    this.service.getparams(url1, param).subscribe((result: any) => {
      this.calculate_deduction[0] = result.Getcomponentamount[0];
      const component_flag_employer = this.calculate_deduction[0].component_flag_employer;
      const component_flag = this.calculate_deduction[0].component_flag;
      const component_percentage = this.calculate_deduction[0].component_percentage;
      const component_amount = this.calculate_deduction[0].component_amount;
      const component_percentage_employer = this.calculate_deduction[0].component_percentage_employer;
      const component_amount_employer = this.calculate_deduction[0].component_amount_employer;
      const componentgroup_name = this.calculate_deduction[0].componentgroup_name;
      if (componentgroup_name == "ESI") {
        this.basic_salary = this.reactiveFormadd.get('gross_salary')?.value;

      }
      else {
        this.basic_salary = this.reactiveFormadd.get('basic_salary')?.value;

      }

      debugger;
      if (component_flag == "Y") {
        this.deduction_list[n].demployee_contribution = Number((this.basic_salary) * (component_percentage / 100)).toFixed(2);
        this.reactiveFormadd.addControl(`demployee_contribution_${n}`, new FormControl(this.deduction_list[n].demployee_contribution));
      }
      else {
        this.deduction_list[n].demployee_contribution = Number(component_amount).toFixed(2);
        this.reactiveFormadd.addControl(`demployee_contribution_${n}`, new FormControl(this.deduction_list[n].demployee_contribution));
      }

      if (component_flag_employer == "Y") {

        this.deduction_list[n].demployer_contribution = Number((this.basic_salary) * (component_percentage_employer / 100)).toFixed(2);
        this.reactiveFormadd.addControl(`demployer_contribution_${n}`, new FormControl(this.deduction_list[n].demployer_contribution));
      } else {

        this.deduction_list[n].demployer_contribution = Number(component_amount_employer).toFixed(2);
        this.reactiveFormadd.addControl(`demployer_contribution_${n}`, new FormControl(this.deduction_list[n].demployer_contribution));
      }

    });

  }

  fetchdetails1() {
    debugger;
    this.buttonClicked = !this.buttonClicked;

    this.Addtictionsummary();
    this.deductionsummary();
    this.Otherssummary();
  }
}


