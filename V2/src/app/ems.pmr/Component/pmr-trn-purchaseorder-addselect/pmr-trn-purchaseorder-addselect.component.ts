import { Component } from '@angular/core';
import { FormBuilder,FormGroup,FormControl,Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
interface purchaseorderadd{

}
@Component({
  selector: 'app-pmr-trn-purchaseorder-addselect',
  templateUrl: './pmr-trn-purchaseorder-addselect.component.html',
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class PmrTrnPurchaseorderAddselectComponent {
  mdlBranchName:any;
  purchaseorderadd !:purchaseorderadd;
  purchaseorderaddform : FormGroup | any;
  branch_list : any[]=[];
   
  purchaseorder_list:any[]=[];
  responsedata: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService, public service: SocketService, private route: ActivatedRoute,private router:Router) 
  {    this.purchaseorderadd = {} as purchaseorderadd;
  this.purchaseorderaddform = new FormGroup({
    branch_name: new FormControl('', Validators.required),
    branch_gid: new FormControl(''),
  });
    
}

  
    ngOnInit(): void {
      
      var url = 'PmrTrnPurchaseOrder/GetBranch'
      this.service.get(url).subscribe((result:any)=>{
        this.responsedata = result;
        this.branch_list =this.responsedata.GetBranch;
        this.purchaseorderaddform.get("branch_gid")?.setValue( result.GetBranch[0].branch_gid);
  
       });

    }
    

    GetOnChangeLocation() { 
debugger
      let branch_gid = this.purchaseorderaddform.value.branch_name.branch_gid;
  let param = {
    branch_gid: branch_gid
  }
    
      var url = 'PmrTrnPurchaseorderAddselect/GetPurchaseOrderSummaryaddselect'
      this.service.getparams(url,param).subscribe((result: any) => {
        $('#purchaseorder_list').DataTable().destroy();
        this.responsedata = result;
        this.purchaseorder_list = this.responsedata.GetPurchaseOrder_lists1;
        console.log(this.purchaseorder_list )
        setTimeout(() => {
          $('#purchaseorder_list').DataTable();
        }, 1);
      });

    }
  get branch_name() {

    return this.purchaseorderaddform.get('branch_name')!;

  };

  selectAllChecked = false;

selectAll(event: any) {
  this.selectAllChecked = event.target.checked;
  this.purchaseorder_list.forEach(data => {
    data.selected = this.selectAllChecked;
  });
}

onRowSelect(data: any) {
  data.selected = !data.selected;
  this.selectAllChecked = this.purchaseorder_list.every(item => item.selected);
}

getSelectedGids(): number[] {
  return this.purchaseorder_list.filter(data => data.selected).map(data => data.purchaserequisition_gid);
}

back(){
  this.router.navigate(['/pmr/PmrTrnPurchaseorderSummary'])
}



addPurchaseOrder(purchaserequisition_gid:any) {
  debugger
  const branchgid = this.purchaseorderaddform.value.branch_name.branch_gid;
 // const purchaserequisition_gid = this.purchaseorder_list.map(item => item.selected);
  
  

  // const requestData = {
    // this.branch_gid= branchgid,
    purchaserequisition_gid= purchaserequisition_gid
  // };

  // const url = 'PmrTrnPurchaseorderAddselect/PostPoaddSubmit';
  // this.NgxSpinnerService.show();


  // this.service.getparams(url, requestData).subscribe((result: any) => {
  //   this.NgxSpinnerService.hide();
  //   window.scrollTo({ top: 0 });

  //   if (result.status === false) {
  //     // this.ToastrService.warning(result.message);
    // } else {
      // this.ToastrService.success(result.message);

      const secretKey = 'storyboarderp';
      const branch_gidStr =this.purchaseorderaddform.value.branch_name.branch_gid;
      const selectedItemsStr = purchaserequisition_gid
      const encryptedBranchGid = AES.encrypt(branch_gidStr, secretKey).toString();
      const encryptedSelectedItems = AES.encrypt(selectedItemsStr, secretKey).toString();
      this.router.navigate(['/pmr/PmrTrnPurchaseorderAddconfirm', encryptedBranchGid, encryptedSelectedItems]);
    // }
  // });
}
}