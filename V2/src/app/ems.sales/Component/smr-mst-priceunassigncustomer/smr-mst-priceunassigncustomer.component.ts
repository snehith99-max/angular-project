import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
export class IAssignEmployee {
  pricesegment_gid:any;
  pricesegmentunassign:any;
}
@Component({
  selector: 'app-smr-mst-priceunassigncustomer',
  templateUrl: './smr-mst-priceunassigncustomer.component.html',
  styleUrls: ['./smr-mst-priceunassigncustomer.component.scss']
})
export class SmrMstPriceunassigncustomerComponent {
  responsedata:any;
  pricesegmentunassign:any[] = [];
  selection = new SelectionModel<IAssignEmployee>(true, []);
  leave_name:any;
  leave_code:any;
  pricesegment_gid: any;
  pick:Array<any> = [];
  CurObj: IAssignEmployee = new IAssignEmployee();


  constructor(    
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    private NgxSpinnerService: NgxSpinnerService
     ){}

     ngOnInit(): void { 
  
      //// Summary Grid//////
      const pricesegment_gid = this.router.snapshot.paramMap.get('pricesegment_gid');
      this.pricesegment_gid = pricesegment_gid;
      const secretKey = 'storyboarderp';
      const deencryptedParam = AES.decrypt(this.pricesegment_gid, secretKey).toString(enc.Utf8);    
      this.pricesegment_gid = deencryptedParam
      let param = {
        pricesegment_gid: deencryptedParam
      }

    
        var url = 'SmrMstPricesegmentSummary/unassigncustomer'
        this.NgxSpinnerService.show()
        this.service.getparams(url,param).subscribe((result: any) => {
        this.responsedata = result;
        this.pricesegmentunassign = this.responsedata.pricesegmentunassign;
        this.NgxSpinnerService.hide();
          setTimeout(() => {
            $('#pricesegmentunassign').DataTable();
            }, );
        
        });        
      }

      
     isAllSelected() {
      const numSelected = this.selection.selected.length;
      const numRows = this.pricesegmentunassign.length;
      return numSelected === numRows;
    }
    masterToggle() {
      this.isAllSelected() ?
        this.selection.clear() :
        this.pricesegmentunassign.forEach((row: IAssignEmployee) => this.selection.select(row));
    }
      unassign(){
        debugger;
          this.pick = this.selection.selected  
          this.CurObj.pricesegmentunassign = this.pick
          this.CurObj.pricesegment_gid=this.pricesegment_gid
            if (this.CurObj.pricesegmentunassign.length === 0) {
            this.ToastrService.warning("Select atleast one employee");
            return;
          } 
      
          debugger
          this.NgxSpinnerService.show();
            var url = 'SmrMstPricesegmentSummary/PriceSegmetUnAssignSubmit';  
            this.service.post(url, this.CurObj).subscribe((result: any) => {
              if (result.status === false) {
                this.ToastrService.warning(result.message);
                this.NgxSpinnerService.hide();
              } else {
                this.ToastrService.success(result.message);
                this.route.navigate(['/smr/SmrMstPricesegmentSummary'])
                this.NgxSpinnerService.hide();                
              }
            });        
          this.selection.clear();
    }
}






  


