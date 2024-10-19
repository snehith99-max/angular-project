import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnOutletmanageEmployeeassignComponent } from './otl-trn-outletmanage-employeeassign.component';

describe('OtlTrnOutletmanageEmployeeassignComponent', () => {
  let component: OtlTrnOutletmanageEmployeeassignComponent;
  let fixture: ComponentFixture<OtlTrnOutletmanageEmployeeassignComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnOutletmanageEmployeeassignComponent]
    });
    fixture = TestBed.createComponent(OtlTrnOutletmanageEmployeeassignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
