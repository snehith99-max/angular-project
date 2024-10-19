import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptGrndetailreportComponent } from './ims-rpt-grndetailreport.component';

describe('ImsRptGrndetailreportComponent', () => {
  let component: ImsRptGrndetailreportComponent;
  let fixture: ComponentFixture<ImsRptGrndetailreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptGrndetailreportComponent]
    });
    fixture = TestBed.createComponent(ImsRptGrndetailreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
