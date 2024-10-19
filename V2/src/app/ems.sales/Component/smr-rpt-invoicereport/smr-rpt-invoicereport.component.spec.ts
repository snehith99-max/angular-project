import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptInvoicereportComponent } from './smr-rpt-invoicereport.component';

describe('SmrRptInvoicereportComponent', () => {
  let component: SmrRptInvoicereportComponent;
  let fixture: ComponentFixture<SmrRptInvoicereportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptInvoicereportComponent]
    });
    fixture = TestBed.createComponent(SmrRptInvoicereportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
