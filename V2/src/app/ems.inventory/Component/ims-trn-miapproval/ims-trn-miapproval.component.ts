import { Component, Input } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-ims-trn-miapproval',
  templateUrl: './ims-trn-miapproval.component.html',
  styleUrls: ['./ims-trn-miapproval.component.scss'],
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
export class ImsTrnMIapprovalComponent {

  materialindent_list:any;
  productsummary_list:any;
  responsedata: any;
  showOptionsDivId: any; 
  isClicked = false;

  constructor(public service :SocketService,private router:Router,private route:Router,private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {  
  }
  @Input() number: number = 1;
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetIssueMaterialSummary();
  }
  GetIssueMaterialSummary() {
    var url = 'ImsTrnMaterialIndent/MatrialIndentApprovalsummary'
    this.service.get(url).subscribe((result: any) => {
      debugger
      $('#materialindentlist').DataTable().destroy();
      this.responsedata = result;
      this.materialindent_list = this.responsedata.materialindentsummary_list;
      setTimeout(() => {
        $('#materialindentlist').DataTable();
      }, 1);
    });
  }
  onview(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/ims/ImsTrnMIapprovalreview',encryptedParam]);
  }
  Getproduct(materialrequisition_gid: any){
    let param = {
      materialrequisition_gid:materialrequisition_gid
    } 
    var url = 'ImsTrnMaterialIndent/GetMIViewProduct'
    this.service.getparams(url,param).subscribe((result: any) => {
      debugger
      $('#productsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.productsummary_list = this.responsedata.productsummary_list;
      setTimeout(() => {
        $('#productsummary_list').DataTable();
      }, 1);
    });
  }
}
