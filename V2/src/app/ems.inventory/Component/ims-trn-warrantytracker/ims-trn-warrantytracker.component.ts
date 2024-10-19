import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-ims-trn-warrantytracker',
  templateUrl: './ims-trn-warrantytracker.component.html',
  styleUrls: ['./ims-trn-warrantytracker.component.scss']
})
export class ImsTrnWarrantytrackerComponent {

  product_list: any[] = [];
  Getupdaysproduct_list: any[] = [];
  Getexpiryproduct_list: any[] = [];
  showOptionsDivId: any;
  paramvalue: any;
  setdaysform!: FormGroup;

  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    private router: ActivatedRoute,
    private route: Router,
    public service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) { }

  ngOnInit(): void {

    this.setdaysform = new FormGroup({
      setdaysproduct: new FormControl(''),
    })

    var productapi = 'PmrMstProduct/GetProductSummary';
    this.service.get(productapi).subscribe((result: any) => {
      this.product_list = result.product_list;
    });
    var Getupdaysapi = 'ExpiryTracker/GetUpdaysSummary';
    this.service.get(Getupdaysapi).subscribe((result: any) => {
      this.Getupdaysproduct_list = result.Getupdaysproduct_list;
    });
    var Getexpirysapi = 'ExpiryTracker/GetExpirySummary';
    this.service.get(Getexpirysapi).subscribe((result: any) => {
      this.Getexpiryproduct_list = result.Getexpiryproduct_list;
    });
  }
  toggleOptions(product_gid: any) {
    if (this.showOptionsDivId === product_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = product_gid;
    }
  }
  onupdate() {
    let param = {
      expirydate: this.setdaysform.value.setdaysproduct,
      product_gid: this.paramvalue,
    }
    var postapi = 'ExpiryTracker/PostSetProductdays';
    this.service.post(postapi, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      }
      else {
        this.ngOnInit();
        this.ToastrService.success(result.message);
      }
    });
  }
  open(product_gid: any) {
    this.paramvalue = product_gid;
  }
}
