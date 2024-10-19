import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface Isalarycomponent{
  salarycomponent_gid: string;
  componentgroup_gid: string;
  componentgroup_name: string;
  component_name: string;
  component_code: string;
    }

@Component({
  selector: 'app-pay-mst-salarycomponentadd',
  templateUrl: './pay-mst-salarycomponentadd.component.html',
  styleUrls: ['./pay-mst-salarycomponentadd.component.scss']
})
export class PayMstSalarycomponentaddComponent {
  showInput0: boolean = false;
  showInput: boolean = false;
  responsedata: any;
  mdlcomponentgroup_name:any
  showcustomizedropdown0:boolean=false;
   showformuladropdown:boolean=false;
   Code_Generation: any;
   showInputField: boolean | undefined;   
  cboselectedComponent: any;
  cboselectedComponent1:any;
  showInput1: boolean = false;
  showInput2: boolean = false;
  showInputs: boolean = false;
  inputValue: string = ''
  inputValue1: string = ''
  inputValue2: string = ''
  is_percent :any;
  is_percentage: any;
  reactiveForm: any;
  componentgrouplist: any[] = [];
  componentnamelist:any[]=[];
  additionvariablelist:any[]=[];
  deductionvariablelist:any[]=[];
  salarycomponent!: Isalarycomponent;
  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.salarycomponent = {} as Isalarycomponent;
  }
  ngOnInit(): void { 
    // Form values for Add popup/////
    this.reactiveForm = new FormGroup({
      component_code:new FormControl('',[Validators.required]),
      component_name: new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      component_type: new FormControl('Addition'),
      contribution_type: new FormControl('EMPLOYEE ONLY'),
      // component_code_auto: new FormControl(''),
      // component_code_manual: new FormControl('', [Validators.required,]),
      affect_in: new FormControl('Basic'),
      lop_deduction: new FormControl('Y'),
      statutory_pay: new FormControl(''),
      other_componenttype: new FormControl('Addition'),
      is_percent: new FormControl('Y'),
      is_percentage: new FormControl('Y'),
      employee_percent: new FormControl(''), 
      employer_percentage: new FormControl(''),
      employee_amount: new FormControl(''),
      employer_amount: new FormControl(''),
      customizecomponent:new FormControl(''),
      formulaaffect_in:new FormControl('Basic'),
      operatoraffect_in:new FormControl('add'),
      additionvariblecomponent:new FormControl(''),
      Code_Generation: new FormControl(''),
      componentgroup_name : new FormControl('', [Validators.required,Validators.minLength(1),]),     
        salarycomponent_gid: new FormControl(''),
        componentgroup_gid: new FormControl(''),   
    });    
    var api='PayMstSalaryComponent/GetComponentGroupDtl'
    this.service.get(api).subscribe((result:any)=>{
    this.componentgrouplist = result.GetComponentGroupDtl;
    //console.log(this.componentgrouplist)
   });
  } 
   ////////////Add popup validation////////
get componentgroup_name() {
  return this.reactiveForm.get('componentgroup_name')!;
}
get component_code_manual() {
  return this.reactiveForm.get('component_code_manual')!;
}
get component_name() {
  return this.reactiveForm.get('component_name')!;
}
get component_code() {
  return this.reactiveForm.get('component_code')!;
}

showTextBox0(event: Event) {
  const target = event.target as HTMLInputElement;
  this.showInput0 = target.value === 'EMPLOYEE ONLY' ;  
}
showTextBox(event: Event) {
  const target = event.target as HTMLInputElement;
  this.showInput = target.value === 'EMPLOYER ONLY' ;
  this.showInputs = target.value === 'EMPLOYER ONLY' ;
}
showTextBox1(event: Event) {
  const target = event.target as HTMLInputElement;
  this.showInput1 = target.value === 'BOTH' ;
}
showTextBoxsta(event: Event) {
  const target = event.target as HTMLInputElement;
  this.showInputs = target.value === 'BOTH' ;
}
showTextBox2(event: Event) {
  const target = event.target as HTMLInputElement;
  this.showInput2 = target.value === 'Others' ;
}
  onsubmit(){

    if (this.reactiveForm.value.component_name != null && this.reactiveForm.value.component_name != '')
      {
    for (const control of Object.keys(this.reactiveForm.controls)) {
    this.reactiveForm.controls[control].markAsTouched();
    }
   this.reactiveForm.value;
   var url1 = 'PayMstSalaryComponent/PostComponent'
          this.service.postparams(url1, this.reactiveForm.value).subscribe((result: any) => {
  
              if (result.status == false) {
  
                 this.ToastrService.warning(result.message)
              // this.LoanSummary();
              }
  
              else {
                this.reactiveForm.get("salarycomponent_gid")?.setValue(null);
                this.reactiveForm.get("componentgroup_name")?.setValue(null);
                this.reactiveForm.get("component_code")?.setValue(null);
                this.reactiveForm.get("component_name")?.setValue(null);
                this.reactiveForm.get("component_type")?.setValue(null);
                this.reactiveForm.get("contribution_type")?.setValue(null);
                this.reactiveForm.get("affect_in")?.setValue(null);
                this.reactiveForm.get("lop_deduction")?.setValue(null);
                this.reactiveForm.get("statutory_pay")?.setValue(null);
                this.reactiveForm.get("other_componenttype")?.setValue(null);
                this.reactiveForm.get("is_percent")?.setValue(null);
                this.reactiveForm.get("employee_percent")?.setValue(null);
                this.reactiveForm.get("employee_amount")?.setValue(null);
                this.reactiveForm.get("is_percentage")?.setValue(null);
                this.reactiveForm.get("employer_percentage")?.setValue(null);
                this.reactiveForm.get("employer_amount")?.setValue(null);           
                
               
                  this.ToastrService.success(result.message);  
              }
              this.router.navigate(['/payroll/PayMstSalaryComponent']);

            });
          }
       
        }

  onback(){
    this.router.navigate(['/payroll/PayMstSalaryComponent']);
  } 
  showdropdown(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showcustomizedropdown0 = target.value === 'Customize' ;
    var api='PayMstSalaryComponent/GetCustomizeComponent'
    this.service.get(api).subscribe((result:any)=>{
      this.responsedata = result;

    this.componentnamelist = this.responsedata.GetComponentnamedropdown;
    //console.log(this.componentgrouplist)
   });
  }
  showmultipledropdown(event:Event){
    const target = event.target as HTMLInputElement;
    this.showformuladropdown = target.value === 'Formula' ;
    var api='PayMstSalaryComponent/GetAddtionVariable'
    this.service.get(api).subscribe((result:any)=>{
      this.responsedata = result;

    this.additionvariablelist = this.responsedata.Getadditioncomponentvariable;
    //console.log(this.componentgrouplist)
   });
  
  }
  toggleInputField() {
    this.showInputField = this.Code_Generation === 'N'; 
  }

}

