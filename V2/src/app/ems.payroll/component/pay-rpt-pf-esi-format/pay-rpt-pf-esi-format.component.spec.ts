import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayRptPfEsiFormatComponent } from './pay-rpt-pf-esi-format.component';

describe('PayRptPfEsiFormatComponent', () => {
  let component: PayRptPfEsiFormatComponent;
  let fixture: ComponentFixture<PayRptPfEsiFormatComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayRptPfEsiFormatComponent]
    });
    fixture = TestBed.createComponent(PayRptPfEsiFormatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
