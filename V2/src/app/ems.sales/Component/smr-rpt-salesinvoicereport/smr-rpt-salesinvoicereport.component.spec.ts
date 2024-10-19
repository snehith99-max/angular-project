import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptSalesinvoicereportComponent } from './smr-rpt-salesinvoicereport.component';

describe('SmrRptSalesinvoicereportComponent', () => {
  let component: SmrRptSalesinvoicereportComponent;
  let fixture: ComponentFixture<SmrRptSalesinvoicereportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptSalesinvoicereportComponent]
    });
    fixture = TestBed.createComponent(SmrRptSalesinvoicereportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
