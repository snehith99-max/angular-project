import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-ims-rpt-highcost',
  templateUrl: './ims-rpt-highcost.component.html',
  styleUrls: ['./ims-rpt-highcost.component.scss']
})
export class ImsRptHighcostComponent {

  responsedata: any;
  highcost_list: any[] = [];
  mdlBranchName: any;
  location_list: any[] = [];
  closingstock!: FormGroup;

  constructor(private formBuilder: FormBuilder, private router: Router,
              private ToastrService: ToastrService, public service: SocketService,
              public NgxSpinnerService: NgxSpinnerService) {}

  ngOnInit(): void {
    this.GethighCostreport();
  }

  GethighCostreport() {
    const url = 'ImsRptHighCostReport/GetHighcostreport';
    this.NgxSpinnerService.show();
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.highcost_list = this.responsedata.highcost_list;
      setTimeout(() => {
        $('#highcost_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });
  }

  interpolateColors(value: number): string {
    const startColor = '#39ff42';
    const endColor = '#ff3939';
    const percent = Math.min(Math.max(value / 100, 0), 1);
    const color1 = this.hexToRgb(startColor);
    const color2 = this.hexToRgb(endColor);
    const r = Math.round(color1.r + (color2.r - color1.r) * percent);
    const g = Math.round(color1.g + (color2.g - color1.g) * percent);
    const b = Math.round(color1.b + (color2.b - color1.b) * percent);
    return `rgb(${r}, ${g}, ${b})`;
  }

  hexToRgb(hex: string): { r: number, g: number, b: number } {
    const bigint = parseInt(hex.slice(1), 16);
    return {
      r: (bigint >> 16) & 255,
      g: (bigint >> 8) & 255,
      b: bigint & 255
    };
  }
}
