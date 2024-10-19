import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder,FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';
import { data } from 'jquery';


@Component({
  selector: 'app-pay-mst-empgradeconfirm',
  templateUrl: './pay-mst-empgradeconfirm.component.html',
  styleUrls: ['./pay-mst-empgradeconfirm.component.scss']
})
export class PayMstEmpgradeconfirmComponent {
  salary_mode : any;
  employeegid: any;
  gradeconfirm!: FormGroup;
  salarygrade_list: any[] = [];
  employee_list: any[] = [];
  template_name:any;
  gross_salary : any;
  submitted = false;
  buttonClicked: boolean | undefined;
  addition_list: any[] = [];
  deduction_list : any[] = [];
  others_list : any[] = [];
  responsedata: any;
  overallnetsalary: any;
  overallctcamount: any;
  parameterValue: any;
  empgradeEditForm : FormGroup | any;
  editcomponentgroup_name: any;
  editcomponent_name: any;
  editcomponent_amount:any;
  editcomponent_amount_employer: any;
  componentList : any [] = [];
  componentfetchlist : any [] = [];
  componentamountlist: any [] = [];
  mdlcomponent: any;
  mdlcomponentgroup: any;
  paramstring: any;
  showInputs: boolean = false;


  mdlcomponentname: any;
  percentage: any;
  componentform : FormGroup | any;
  componenttypelist: any [] = [];
  componentgrouplist: any [] = [];
  componentnamelist: any [] = [];
  deencryptedParam :any;


  editempgradeFormData = {
    editcomponentgroup_name: '',
    editcomponent_name: '',
    editcomponent_amount: '',
    editcomponent_amount_employer: '',
  };


 constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute,
    private formBuilder: FormBuilder,    public NgxSpinnerService:NgxSpinnerService) {  }

    ngOnInit() : void{

      const employee_gid = this.router.snapshot.paramMap.get('employee_gid');
    this.employeegid = employee_gid;
  
    const secretKey = 'storyboarderp';
    this.deencryptedParam = AES.decrypt(this.employeegid, secretKey).toString(enc.Utf8);

    // Split the param string into individual IDs
    const employeeIds = this.deencryptedParam.split('+');

    // Create an array of objects with the employee_gid values
     this.employee_list = employeeIds.map((id: any) => ({ employee_gid: id }));

      this.gradeconfirm = new FormGroup({

        template_name :  new FormControl(''),
        salary_mode :  new FormControl(''),

        gross_salary: new FormControl(''),
        net_salary: new FormControl(''),
        grosssalary: new FormControl(''),
        ctc: new FormControl(''),
        employee_gid: new FormControl(''),
        basic_salary: new FormControl(''),

      })

      this.empgradeEditForm = new FormGroup({
        editcomponentgroup_name :  new FormControl(''),
        editcomponent_name :  new FormControl(''),
        editcomponent_amount :  new FormControl(''),
        editcomponent_amount_employer :  new FormControl(''),
        salarygradetmpdtl_gid:new FormControl('')

      })

      this.componentform =  new FormGroup({
        component_type : new FormControl(''),
        componentgroup_name: new FormControl(''),
        component_name: new FormControl(''),
        component_amount: new FormControl(''),
        component_amount_employer: new FormControl('')


      })


    var url = 'PayMstGradeConfirm/Getsalarygradetemplatedropdown';
    this.service.get(url).subscribe((result: any) => {
      this.salarygrade_list = result.gradetemplatedropdown;
    });

    var url = 'PayMstGradeConfirm/Getcomponentlistdropdown'
    this.service.get(url).subscribe((result: any) => {
      this.componentList = result.componentlist;
    });


    var url = 'PayMstGradeConfirm/Getcomponenttypedropdown'
    this.service.get(url).subscribe((result: any) => {
      this.componenttypelist = result.componenttypelist;
    });


    }
   
  //  GetAdditionsummary(){
  //    var url = 'PayMstGradeConfirm/additionsummarybind'
  //    let param={
 
  //      salarygradetemplategid:this.gradeconfirm.value.template_name,
  //      gross_salary:this.gradeconfirm.value.gross_salary
  //    }
  //    this.service.getparams(url, param).subscribe((result: any) => {
  //      this.responsedata = result;
  //      this.addition_list = this.responsedata.Addsummarybind_list;
 
  //      this.gradeconfirm.get("basic_salary")?.setValue(this.addition_list[0].basicsalay);
  //     });
  //  }

   GetDeductionsummary(){
    var url = 'PayMstGradeConfirm/Deductionsummarybind'
    let param={

      salarygradetemplategid:this.gradeconfirm.value.template_name,
      gross_salary:this.gradeconfirm.value.gross_salary
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.addition_list = this.responsedata.Addsummarybind_list;

      this.deduction_list = this.responsedata.DedSummarybind_list;
      this.others_list = this.responsedata.OthersSummarybind_list;
  
      this.overallnetsalary = this.responsedata.overallnetsalary;
      this.overallctcamount = this.responsedata.ctc

      this.gradeconfirm.get("net_salary")?.setValue(this.overallnetsalary);
      this.gradeconfirm.get("ctc")?.setValue(this.overallctcamount);
  
    });
   }

  openModaldelete(salarygradetmpdtl_gid: any, salarygradetemplate_gid: any) {

    const index = this.addition_list.indexOf(data);
    if (index !== -1) {
        this.addition_list.splice(index, 1);
    }
    
    let param ={
      salarygradetmpdtl_gid : salarygradetmpdtl_gid,
      salarygradetemplate_gid: salarygradetemplate_gid
    }
    this.paramstring =  param    
  }

  ondelete(){
    this.NgxSpinnerService.show();

    console.log(this.paramstring);

    var url3 = 'PayMstGradeConfirm/getDeleteComponent'

    this.service.getparams(url3, this.paramstring).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();

        this.ToastrService.warning(result.message)

      }
      else {
        this.NgxSpinnerService.hide();
      this.ToastrService.success(result.message)
      }
    });
    //this.GetAdditionsummary();
    this.GetDeductionsummary();
    // this.GetOtherssummary();
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

  fetchdetails1(){
    this.buttonClicked = !this.buttonClicked;

    //this.GetAdditionsummary();
    this.GetDeductionsummary();
 
   

  }

  submit(){
  
    var params={ 
      Addsummarybind_list : this.addition_list,
      DedSummarybind_list: this.deduction_list,
      OthersSummarybind_list: this.others_list,
      employee_lists:this.employee_list,
      gross_salary : this.gradeconfirm.value.gross_salary,
      ctc : this.gradeconfirm.value.ctc,
      net_salary : this.gradeconfirm.value.net_salary,
      template_name : this.gradeconfirm.value.template_name,
      salary_mode:this.gradeconfirm.value.salary_mode,
      BasicSalary : this.gradeconfirm.value.basic_salary

  
    }
    console.log(params)
   
    var url = 'PayMstGradeConfirm/Postemployee2grade'
    this.NgxSpinnerService.show();
      this.service.postparams(url,params).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.route.navigate(['/payroll/PayMstEmployeetemplatesummary']);
       }
       else{
        this.ToastrService.success(result.message)
        this.route.navigate(['/payroll/PayMstEmployeetemplatesummary']);
       }
      });

  }

  update(){
    debugger;
    var url = 'PayMstGradeConfirm/Updateempgrade';
    this.service.post(url, this.empgradeEditForm.value).subscribe((result:any) => {
      if(result.status == true){
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        // this.empgradeEditForm.reset();
        // this.gradeconfirm.reset();

       
      }
      else {
            
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
        // this.empgradeEditForm.reset();
        // this.gradeconfirm.reset();
      }
      // this.ngOnInit();
    })
      
    // this.GetAdditionsummary();
    this.GetDeductionsummary();
  }

  editempgrade(parameter: string) {
    debugger;
    this.parameterValue = parameter
    this.empgradeEditForm.get("editcomponentgroup_name")?.setValue(this.parameterValue.componentgroup_name);
    this.empgradeEditForm.get("editcomponent_name")?.setValue(this.parameterValue.salarycomponent_name);
    this.empgradeEditForm.get("editcomponent_amount")?.setValue(this.parameterValue.salarycomponent_amount); 
    this.empgradeEditForm.get("editcomponent_amount_employer")?.setValue(this.parameterValue.salarycomponent_amount_employer);
    this.empgradeEditForm.get("salarygradetmpdtl_gid")?.setValue(this.parameterValue.salarygradetmpdtl_gid);  
  
}



Submit(){

}
formatCurrency(value:any) {
  const formattedValue = parseFloat(value).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
  return formattedValue;
}

componentSubmit(){

}

componentDetailsFetch(){
  let componenttype = this.componentform.get('component_type')?.value;

    let param = {
      component_type: componenttype
    }

    var url = 'PayMstGradeConfirm/GetOnChangeComponentgroup';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.componentgrouplist = this.responsedata.componentgrouplist;
      
    });

}


componentnamebinding(){
  let componentgroupname = this.componentform.get('componentgroup_name')?.value;

    let param = {
      componentgroup_name: componentgroupname
    }

    var url = 'PayMstGradeConfirm/GetOnChangeComponent';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.componentnamelist = this.responsedata.componentnamelist;
      
    });

}

amountbinding(){
  let componentname = this.componentform.get('component_name')?.value;


    let param = {
      component_name: componentname
    }

    var url = 'PayMstGradeConfirm/GetOnChangeComponentamount';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.componentamountlist = this.responsedata.componentamountlist;
    });
  }


  onSalaryModeChange(){
    debugger;

    this.salary_mode = this.gradeconfirm.value.salary_mode;
    if(this.salary_mode == 'Basic'){
    const secretKey = 'storyboarderp';
    const param = this.deencryptedParam;
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/payroll/PayMstEmployeegradeconfirm',encryptedParam]);
    }
    else if(this.salary_mode == 'Gross'){
      const secretKey = 'storyboarderp';
      const param = this.deencryptedParam;
      const encryptedParam = AES.encrypt(param, secretKey).toString();
      this.route.navigate(['/payroll/PayMstEmpgradeconfirm',encryptedParam])
    }

  }

  showTextBox(event: Event) {
    const target = event.target as HTMLInputElement;
  this.showInputs = target.value === 'Gross';
}


}