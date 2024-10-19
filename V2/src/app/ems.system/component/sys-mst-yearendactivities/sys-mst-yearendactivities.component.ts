import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';


@Component({
  selector: 'app-sys-mst-yearendactivities',
  templateUrl: './sys-mst-yearendactivities.component.html',
  styleUrls: ['./sys-mst-yearendactivities.component.scss']
})
export class SysMstYearendactivitiesComponent {

  responsedata: any;
  yearendactivities_list: any[] = [];
  paramstring: any;

  constructor(public router: Router,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService,
    private service: SocketService,
  ) { }


  ngOnInit(): void {
    this.getfinancialyearsummary()
  }

  getfinancialyearsummary() {
    debugger
    this.NgxSpinnerService.show();
    var api = 'YearEndClose/GetYearendactivities'
    
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.yearendactivities_list = this.responsedata.GetYearEndDetails_list;
      setTimeout(() => {
        $('#yearendactivities_list').DataTable();
        this.NgxSpinnerService.hide() }, 1);
    });
   
  }
  yearend_close(finyear_gid: any, start_year: any) {
    
    let param ={
      finyear_gid : finyear_gid,
      start_year: start_year
    }
    this.paramstring =  param    
  }

  onok()
  {

    this.NgxSpinnerService.show();
    
    console.log(this.paramstring);

    var api2 = 'YearEndClose/PostYearendactivities';

    
    this.service.getparams(api2, this.paramstring).subscribe((result: any) => {
      this.responsedata = result;
      if(result.status == false)
      {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else{
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
      }
    }); 
  }
}

