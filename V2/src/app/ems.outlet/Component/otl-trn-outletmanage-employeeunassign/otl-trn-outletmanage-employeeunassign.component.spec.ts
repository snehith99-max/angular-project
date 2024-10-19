import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnOutletmanageEmployeeunassignComponent } from './otl-trn-outletmanage-employeeunassign.component';

describe('OtlTrnOutletmanageEmployeeunassignComponent', () => {
  let component: OtlTrnOutletmanageEmployeeunassignComponent;
  let fixture: ComponentFixture<OtlTrnOutletmanageEmployeeunassignComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnOutletmanageEmployeeunassignComponent]
    });
    fixture = TestBed.createComponent(OtlTrnOutletmanageEmployeeunassignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
