import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';

interface Iarbitration {
  arbitration_date: string;
  arbitration_no: string;
  arbit_type:string;
  title:string;
  arbitrator:string;
  institute: string;
  created_date: string;
  Status:string;
  arbitration_gid:string; 
}
@Component({
  selector: 'app-law-trn-arbitration',
  templateUrl: './law-trn-arbitration.component.html',
  styleUrls: ['./law-trn-arbitration.component.scss']
})
export class LawTrnArbitrationComponent {
  Arbitration!: Iarbitration;
  ArbitrationList:any;
  reactiveForm!: FormGroup;

  constructor(private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService, private ToastrService: ToastrService) {
    this.Arbitration = {} as Iarbitration;

  }
  ngOnInit(): void {
    this.GetArbitrationSummary();
   this.reactiveForm = new FormGroup({
     arbitration_date: new FormControl(''),
     arbitration_no: new FormControl(''),
     arbit_type: new FormControl(''),
     title: new FormControl(''),
     arbitrator: new FormControl(''),
     institute: new FormControl(''),
     created_date: new FormControl(''),
     Status: new FormControl(''),
   
   });
 
   }
   GetArbitrationSummary(){
    debugger
    this.NgxSpinnerService.show();
    var url= 'LawTrnArbitration/GetArbitrationsummary';
    this.SocketService.get(url).subscribe((result:any)=>{
      console.log(result.arbitration_list);
      if(result.arbitration_list != null){
        $('#ArbitrationSummary').DataTable().destroy();
        this.ArbitrationList = result.arbitration_list;  
        this.NgxSpinnerService.hide();
        setTimeout(()=>{   
          $('#ArbitrationSummary').DataTable();
        }, 1);
      }
      else{
        this.ArbitrationList = result.arbitration_list; 
        setTimeout(()=>{   
          $('#ArbitrationSummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#ArbitrationSummary').DataTable().destroy();
      } 
    });

   }

  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }

}
