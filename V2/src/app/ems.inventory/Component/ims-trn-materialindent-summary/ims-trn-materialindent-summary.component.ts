import { Component, Input } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-ims-trn-materialindent-summary',
  templateUrl: './ims-trn-materialindent-summary.component.html',
  styleUrls: ['./ims-trn-materialindent-summary.component.scss'],
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
export class ImsTrnMaterialindentSummaryComponent {

  materialindent_list:any;
  productsummary_list:any;
  responsedata: any;
  showOptionsDivId: any; 
  MICount_list: any[] = [];
  response_data: any;
  totalcount: any;
  prioritycount: any;
  available_count: any;
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
    this.GetMIcount();
  }
  GetIssueMaterialSummary() {
    var url = 'ImsTrnMaterialIndent/MatrialIndentsummary'
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
  reloadPage(): void {
    window.location.reload();
  }
  
  GetMIcount() {

    var url = 'ImsTrnMaterialIndent/GetMICount';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.MICount_list = this.response_data.MICount;
      this.totalcount = this.MICount_list[0].totalcount;
      this.prioritycount = this.MICount_list[0].prioritycount;
      this.available_count = this.MICount_list[0].available_count
   

    });
  }


  filterByPriority() {
    // Filter the list based on priority
    this.materialindent_list = this.responsedata.materialindentsummary_list.filter(
      (item: any) => item.priority === 'Y'
    );
    this.number = 2;
    this.isClicked = true;
    setTimeout(() => {
      $('#materialindentlist').DataTable();
    }, 1);
  }


  filterByAvailable() {
    this.materialindent_list = this.responsedata.materialindentsummary_list.filter(
      (item: any) => item.available === 'Y'
    );
    this.number = 3;
    setTimeout(() => {
      $('#materialindentlist').DataTable();
    }, 1);
  }
  onadd(){
    this.router.navigate(['/ims/ImsTrnMaterialindentAdd']);
  }
  PrintPDF(materialrequisition_gid: any) {
    debugger;
    const api = 'ImsTrnMaterialIndent/GetmaterialindentRpt';
    this.NgxSpinnerService.show()
    let param = {
      materialrequisition_gid:materialrequisition_gid
    } 
    this.service.getparams(api,param).subscribe((result: any) => {
      if(result!=null){
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }
  onview(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/ims/ImsTrnMaterialIndentView',encryptedParam]);
  }
  issue(params:any){
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/ims/ImsTrnMaterialindentIssue',encryptedParam]);
  }
  trackstatus(params:any){
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/ims/ImsTrnStatustrack',encryptedParam]);
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
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
  
  getColor(buttonNumber: number): any {
    if (buttonNumber === this.number) {
      if (buttonNumber === 1) {
        return { 'background': 'linear-gradient(to right, #ffffff, #f8ceab)' };
      } else if (buttonNumber === 2) {
        return { 'background': this.isClicked ? 'linear-gradient(to right, #ffffff, #ffa6a6)' : 'linear-gradient(to right, #ffffff, #f8aea5)' };
      } else if (buttonNumber === 3) {
        return { 'background': this.isClicked ? 'linear-gradient(to right, #ffffff, #d1fbd1)' : 'linear-gradient(to right, #ffffff, #fbd1d1)', };
      }
    }
    return {};  
  }

  setNumber(buttonNumber: number): void {
    this.number = buttonNumber;
  }
}
