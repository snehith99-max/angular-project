import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlRptOutletdaytrackerreportComponent } from './otl-rpt-outletdaytrackerreport.component';

describe('OtlRptOutletdaytrackerreportComponent', () => {
  let component: OtlRptOutletdaytrackerreportComponent;
  let fixture: ComponentFixture<OtlRptOutletdaytrackerreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlRptOutletdaytrackerreportComponent]
    });
    fixture = TestBed.createComponent(OtlRptOutletdaytrackerreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
