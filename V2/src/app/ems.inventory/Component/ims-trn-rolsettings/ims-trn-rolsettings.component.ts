import { Component } from '@angular/core';
import { FormGroup,FormBuilder,FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-ims-trn-rolsettings',
  templateUrl: './ims-trn-rolsettings.component.html',
  styleUrls: ['./ims-trn-rolsettings.component.scss'],

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
export class ImsTrnRolsettingsComponent {
  stock_list:any;
  response_data :any;
  reactiveForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  invoice:any;
  showOptionsDivId: any; 

  constructor(private fb: FormBuilder,public NgxSpinnerService: NgxSpinnerService,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,) {} 
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  ngOnInit(): void {
    this.GetImsMstReorderlevelSummary();
    
  }

  GetImsMstReorderlevelSummary(){

    var api = 'ImsMstReorderlevel/GetImsMstReorderlevelSummary';
    this.NgxSpinnerService.show()
    this.service.get(api).subscribe((result:any) => {
      this.response_data = result;
      this.stock_list = this.response_data.rol_list;
      setTimeout(()=>{  
        $('#stock_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide()
    });
  
  }
  onadd()
  {
        this.router.navigate(['/ims/ImsMstReorderLevelAdd'])
  }
  onedit(rolgid: any) {
       
        const secretKey = 'storyboarderp';
        const param = rolgid;
        const encryptedParam = AES.encrypt(param, secretKey).toString();
        this.router.navigate(['/ims/ImsTrnReorderLevelEdit', encryptedParam]);
}
}

