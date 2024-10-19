import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptProductgroupreportComponent } from './smr-rpt-productgroupreport.component';

describe('SmrRptProductgroupreportComponent', () => {
  let component: SmrRptProductgroupreportComponent;
  let fixture: ComponentFixture<SmrRptProductgroupreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptProductgroupreportComponent]
    });
    fixture = TestBed.createComponent(SmrRptProductgroupreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
