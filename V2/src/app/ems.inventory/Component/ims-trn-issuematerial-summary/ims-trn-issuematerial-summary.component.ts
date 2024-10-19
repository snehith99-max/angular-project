import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-ims-trn-issuematerial-summary',
  templateUrl: './ims-trn-issuematerial-summary.component.html',
  styleUrls: ['./ims-trn-issuematerial-summary.component.scss'],
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
export class ImsTrnIssuematerialSummaryComponent {

  issuematerial_list:any;
  responsedata: any;
  showOptionsDivId: any;
  productsummary_list:any;

  constructor(public service :SocketService,private router:Router,private route:Router,private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {  
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetIssueMaterialSummary();
  }
  GetIssueMaterialSummary(){
    var url = 'ImsTrnIssueMaterial/GetIssueMaterialSummary'
    this.service.get(url).subscribe((result: any) => {
  
      this.responsedata = result;
      this.issuematerial_list = this.responsedata.Getissuematerial_list;
      setTimeout(() => {
        $('#issuematerial_list').DataTable();
      }, 1);
    });
  }
 
  onadd(){
    this.router.navigate(['/ims/ImsTrnDirectissuematerial']);
  }
  onissue(){
    this.router.navigate(['/ims/ImsTrnIssue']);
  }
  PrintPDF(materialissued_gid: any) {
    debugger;
    const api = 'ImsTrnIssueMaterial/GetmaterialissueRpt';
    this.NgxSpinnerService.show()
    let param = {
      materialissued_gid:materialissued_gid
    } 
    this.service.getparams(api,param).subscribe((result: any) => {
      if(result!=null){
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }
  onview(params:any){
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const lspage1 = 'Inv';
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    const lspage = AES.encrypt(lspage1,secretKey).toString();
    this.route.navigate(['/ims/ImsTrnIssueMaterialView',encryptedParam,lspage]);
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  Getproduct(materialissued_gid: any){
    let param = {
      materialissued_gid:materialissued_gid
    } 
    var url = 'ImsTrnIssueMaterial/GetIssueViewProduct'
    this.service.getparams(url,param).subscribe((result: any) => {
      debugger
      $('#productsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.productsummary_list = this.responsedata.issuematerialproduct_list;
      setTimeout(() => {
        $('#productsummary_list').DataTable();
      }, 1);
    });
  }
}
