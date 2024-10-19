import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysRptScreenprivilegereportComponent } from './sys-rpt-screenprivilegereport.component';

describe('SysRptScreenprivilegereportComponent', () => {
  let component: SysRptScreenprivilegereportComponent;
  let fixture: ComponentFixture<SysRptScreenprivilegereportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysRptScreenprivilegereportComponent]
    });
    fixture = TestBed.createComponent(SysRptScreenprivilegereportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
