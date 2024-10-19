import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysRptAudithistoryComponent } from './sys-rpt-audithistory.component';

describe('SysRptAudithistoryComponent', () => {
  let component: SysRptAudithistoryComponent;
  let fixture: ComponentFixture<SysRptAudithistoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysRptAudithistoryComponent]
    });
    fixture = TestBed.createComponent(SysRptAudithistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
