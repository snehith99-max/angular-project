import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysRptAuditreportComponent } from './sys-rpt-auditreport.component';

describe('SysRptAuditreportComponent', () => {
  let component: SysRptAuditreportComponent;
  let fixture: ComponentFixture<SysRptAuditreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysRptAuditreportComponent]
    });
    fixture = TestBed.createComponent(SysRptAuditreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
