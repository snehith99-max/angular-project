import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-hrm-mst-releivingletter',
  templateUrl: './hrm-mst-releivingletter.component.html',
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

export class HrmMstReleivingletterComponent {
  relievingletter_list: any[] = [];
  // data: any;
  parameterValue1: any;
  parameterValue: any;
  releiving_gid: any;

  constructor(private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, private route: Router, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {

  }

  ngOnInit(): void {
    this.relievinglettersummary()
  }

  relievinglettersummary() {
    var api = 'RelievingLetter/GetRelievingLetterSummary';
    this.service.get(api).subscribe((result: any) => {
      this.relievingletter_list = result.RelievingLetterLists;
      setTimeout(() => {
        $('#relievingletter_list').DataTable();
      }, 1);

    });
  }


  ondelete() {
    console.log(this.parameterValue);
    var url = 'RelievingLetter/DeleteRelievingLetter'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message);
        this.ngOnInit();
        
      }
      else {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }

    });

  }

  deletemodal(parameter: string) {
    debugger
    this.parameterValue = parameter
  }

  

  PrintPDF(releiving_gid: string) {
    debugger
          const api = 'AddRelievingLetter/Getrelievingletterpdf';
          this.NgxSpinnerService.show()
          let param = {
            releiving_gid:releiving_gid,
          
          } 
          this.service.getparams(api,param).subscribe((result: any) => {
            if(result!=null){
              this.service.filedownload1(result);
            }
            this.NgxSpinnerService.hide()
          });
    
        }
}