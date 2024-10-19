import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptGrnreportComponent } from './ims-rpt-grnreport.component';

describe('ImsRptGrnreportComponent', () => {
  let component: ImsRptGrnreportComponent;
  let fixture: ComponentFixture<ImsRptGrnreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptGrnreportComponent]
    });
    fixture = TestBed.createComponent(ImsRptGrnreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
