
import { Component } from '@angular/core';
import { FormBuilder,FormGroup,FormControl,Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ExcelService } from 'src/app/Service/excel.service';
interface purchaseorderadd{

}
@Component({
  selector: 'app-pmr-rpt-purchaserequisition-report',
  templateUrl: './pmr-rpt-purchaserequisition-report.component.html',
  styleUrls: ['./pmr-rpt-purchaserequisition-report.component.scss']
})
export class PmrRptPurchaserequisitionReportComponent {
  mdlBranchName:any;
  purchaseorderadd !:purchaseorderadd;
  purchaserequisitionform : FormGroup | any;
  branch_list : any[]=[];
  Getpurchaserequisitionexcel_list:any[]=[];   
  purchaserequisition_list:any[]=[];
  responsedata: any;
  constructor(private formBuilder: FormBuilder, private excelService: ExcelService,private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService, public service: SocketService, private route: ActivatedRoute,private router:Router) 
  {    this.purchaseorderadd = {} as purchaseorderadd;
  this.purchaserequisitionform = new FormGroup({
    branch_name: new FormControl('', Validators.required),
    branch_gid: new FormControl(''),
  });
    
}

  
    ngOnInit(): void {
      
      var url = 'PmrTrnPurchaseRequisition/GetpurchaserequisitionBranch'
      this.service.get(url).subscribe((result:any)=>{
        this.responsedata = result;
        this.branch_list =this.responsedata.GetpurchaserequisitionBranch;
        this.purchaserequisitionform.get("branch_gid")?.setValue( result.GetpurchaserequisitionBranch[0].branch_gid);
  
       });

    }
    

    GetOnChangeLocation() { 
debugger
      let branch_gid = this.purchaserequisitionform.value.branch_name.branch_gid;
  let param = {
    branch_gid: branch_gid
  }
      this.NgxSpinnerService.show();
      var url = 'PmrTrnPurchaseRequisition/GetPurchaserequisitionrpt'
      this.service.getparams(url,param).subscribe((result: any) => {
        $('#purchaserequisition_list').DataTable().destroy();
        this.responsedata = result;
        this.purchaserequisition_list = this.responsedata.Getpurchaserequisition_list;
        console.log(this.purchaserequisition_list )
        setTimeout(() => {
          $('#purchaserequisition_list').DataTable();
        }, 1);
    
        this.NgxSpinnerService.hide();
      });
      var url = 'PmrTrnPurchaseRequisition/GetPurchaserequisitionexcel'
      this.service.getparams(url,param).subscribe((result: any) => {
              this.responsedata = result;
        this.Getpurchaserequisitionexcel_list = this.responsedata.Getpurchaserequisitionexcel_list;
        console.log(this.Getpurchaserequisitionexcel_list )
           
       
      });

    }
  get branch_name() {

    return this.purchaserequisitionform.get('branch_name')!;

  };

  onview(params:any)
  {
    const secretKey = 'storyboarderp';
    const  report ='N'
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/pmr/PmrTrnPurchaseRequisitionView',encryptedParam]) 
  }


  ProductexportExcel() {
    const ProductExcel = this.Getpurchaserequisitionexcel_list.map(item => ({
      // Approved_Date: item.approved_date || '',
      Purchase_Req_No: item.Purchase_Req_No || '',
      Product_code: item.Product_Code || '',
      Product: item.Product_Name || '',
      Unit: item.Unit || '',
      Requested_qty: item.Requested_Qty || '',
      Status: item.Status || '',

    }));


    this.excelService.exportAsExcelFile(ProductExcel,'Purchase_Requisition_Excel');

  }

  
        
      
}