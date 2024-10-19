import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder,FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-pay-mst-editemp2salarygrade',
  templateUrl: './pay-mst-editemp2salarygrade.component.html',
  styleUrls: ['./pay-mst-editemp2salarygrade.component.scss']
})
export class PayMstEditemp2salarygradeComponent {

  editgradeconfirm! : FormGroup;
  salarygrade_list : any[]=[];
  template_name:any;
  gross_salary : any;
  buttonClicked: boolean | undefined;
  submitted = false;
  geteditdetails: any;
  employee2salarygradetemplate_gid : any;
  responsedata: any;
  addition_list : any[] = [];
  deduction_list : any[] = [];
  others_list : any [] = [];
  gradeChanged: boolean = false;
  selectedTemplateName: any;
  overallnetsalary: any;
  overallctcamount: any;
  employee2salarygradetemplate_gid1:any;


constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute,
    private formBuilder: FormBuilder,    public NgxSpinnerService:NgxSpinnerService) {  }

    ngOnInit(): void {

      const employee2salarygradetemplate_gid = this.router.snapshot.paramMap.get('employee2salarygradetemplate_gid');
      this.employee2salarygradetemplate_gid = employee2salarygradetemplate_gid;
      const secretKey = 'storyboarderp';
      const deencryptedParam = AES.decrypt(this.employee2salarygradetemplate_gid, secretKey).toString(enc.Utf8);

      this.employee2salarygradetemplate_gid1 = deencryptedParam;



      this.Geteditgrade2employeedetails();
      this.additionsummary();
      this.deductionsummary();
      this.otherssummary();
      

      this.editgradeconfirm = new FormGroup({
        salary_mode : new FormControl(''),
        template_name : new FormControl('', Validators.required),
        gross_salary :  new FormControl(''),
        employee2salarygradetemplate_gid :  new FormControl(''),
        net_salary : new FormControl(''),
        ctc :  new FormControl('')



      })


var url = 'PayMstEditEmpGrade/Getsalarygradetemplatedropdown';
this.service.get(url).subscribe((result: any) => {
this.salarygrade_list = result.gradetemplatedropdown;
});

      
}

  Geteditgrade2employeedetails(){

  const employee2salarygradetemplate_gid = this.router.snapshot.paramMap.get('employee2salarygradetemplate_gid');
  this.employee2salarygradetemplate_gid = employee2salarygradetemplate_gid;

  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.employee2salarygradetemplate_gid, secretKey).toString(enc.Utf8);
  
  let param = {
    employee2salarygradetemplate_gid: deencryptedParam
  }

    var api = 'PayMstEditEmpGrade/editgrade2employeedetails';

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata=result;
      this.geteditdetails = result;
      this.editgradeconfirm.get("employee2salarygradetemplate_gid")?.setValue(this.geteditdetails.employee2salarygradetemplate_gid);
      this.editgradeconfirm.get("salary_mode")?.setValue(this.geteditdetails.salary_mode);
      this.editgradeconfirm.get("template_name")?.setValue(this.geteditdetails.salarygradetemplate_gid);
      this.editgradeconfirm.get("gross_salary")?.setValue(this.geteditdetails.gross_salary);
      this.editgradeconfirm.get("net_salary")?.setValue(this.geteditdetails.net_salary);
      this.editgradeconfirm.get("ctc")?.setValue(this.geteditdetails.ctc);

    });
  }


  additionsummary(){

    const employee2salarygradetemplate_gid = this.router.snapshot.paramMap.get('employee2salarygradetemplate_gid');
  this.employee2salarygradetemplate_gid = employee2salarygradetemplate_gid;

  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.employee2salarygradetemplate_gid, secretKey).toString(enc.Utf8);

  let param = {
    employee2salarygradetemplate_gid: deencryptedParam,
    salarygradetype: "Addition"

  }

    var url = 'PayMstEditEmpGrade/Editadditionsummary'
    // let param: { employee2salarygradetemplate_gid: any, salarygradetype: any } = {
    //   employee2salarygradetemplate_gid: employee2salarygradetemplate_gid,
    //   salarygradetype: "Addition"
    // };
    this.service.getparams(url, param).subscribe((result: any) => {

      this.responsedata = result;
      this.addition_list = this.responsedata.Editaditional_list;
      this.addition_list = this.responsedata.Editaditional_list;


      setTimeout(() => {
        $('#addition_list').DataTable();
      }, 1);


    });

  }

  deductionsummary(){
    const employee2salarygradetemplate_gid = this.router.snapshot.paramMap.get('employee2salarygradetemplate_gid');
    this.employee2salarygradetemplate_gid = employee2salarygradetemplate_gid;
  
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.employee2salarygradetemplate_gid, secretKey).toString(enc.Utf8);
  
    let param = {
      employee2salarygradetemplate_gid: deencryptedParam,
      salarygradetype: "Deduction"
  
    }

    var url = 'PayMstEditEmpGrade/Editdeductionsummary'
    // let param: { employee2salarygradetemplate_gid: any, salarygradetype: any } = {
    //   employee2salarygradetemplate_gid: this.employee2salarygradetemplate_gid,
    //   salarygradetype: "Deduction"
    // };
    this.service.getparams(url, param).subscribe((result: any) => {

      this.responsedata = result;
      this.deduction_list = this.responsedata.Editdeductional_list;
      this.deduction_list = this.responsedata.Editdeductional_list;


      setTimeout(() => {
        $('#deduction_list').DataTable();
      }, 1);


    });

  }


  otherssummary(){

    const employee2salarygradetemplate_gid = this.router.snapshot.paramMap.get('employee2salarygradetemplate_gid');
    this.employee2salarygradetemplate_gid = employee2salarygradetemplate_gid;
  
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.employee2salarygradetemplate_gid, secretKey).toString(enc.Utf8);
  
    let param = {
      employee2salarygradetemplate_gid: deencryptedParam,
      salarygradetype: "others"
  
    }

    var url = 'PayMstEditEmpGrade/Editotherssummary'
    // let param: { employee2salarygradetemplate_gid: any, salarygradetype: any } = {
    //   employee2salarygradetemplate_gid: this.employee2salarygradetemplate_gid,
    //   salarygradetype: "others"
    // };
    this.service.getparams(url, param).subscribe((result: any) => {

      this.responsedata = result;
      this.others_list = this.responsedata.Editotherslist;
      this.others_list = this.responsedata.Editotherslist;


      setTimeout(() => {
        $('#others_list').DataTable();
      }, 1);


    });
  }




  GetDeductionsummary(){
    var url = 'PayMstGradeConfirm/Deductionsummarybind'
    let param={

      salarygradetemplategid:this.editgradeconfirm.value.template_name,
      gross_salary:this.editgradeconfirm.value.gross_salary
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.addition_list = this.responsedata.Addsummarybind_list;

      this.deduction_list = this.responsedata.DedSummarybind_list;
      this.others_list = this.responsedata.OthersSummarybind_list;
  
      this.overallnetsalary = this.responsedata.overallnetsalary;
      this.overallctcamount = this.responsedata.ctc

      this.editgradeconfirm.get("net_salary")?.setValue(this.overallnetsalary);
      this.editgradeconfirm.get("ctc")?.setValue(this.overallctcamount);
  
    });
   }



  update(){
    debugger;
  
  
    var params={ 
      Addsummarybind_list : this.addition_list,
      DedSummarybind_list: this.deduction_list,
      OthersSummarybind_list: this.others_list,
      gross_salary : this.editgradeconfirm.value.gross_salary,
      ctc : this.editgradeconfirm.value.ctc,
      net_salary : this.editgradeconfirm.value.net_salary,
      salarygradetemplate_gid : this.editgradeconfirm.value.template_name,
      salary_mode:this.editgradeconfirm.value.salary_mode,
      BasicSalary : this.editgradeconfirm.value.basic_salary,
      employee2salarygrade_gid:this.employee2salarygradetemplate_gid1

  
    }
    var url = 'PayMstEditEmpGrade/Updateemployee2grade'
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
        this.GetDeductionsummary();
    }


}
