import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayRptPfreportComponent } from './pay-rpt-pfreport.component';

describe('PayRptPfreportComponent', () => {
  let component: PayRptPfreportComponent;
  let fixture: ComponentFixture<PayRptPfreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayRptPfreportComponent]
    });
    fixture = TestBed.createComponent(PayRptPfreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
