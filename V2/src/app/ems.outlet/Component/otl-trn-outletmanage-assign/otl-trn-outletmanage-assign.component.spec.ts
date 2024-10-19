import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnOutletmanageAssignComponent } from './otl-trn-outletmanage-assign.component';

describe('OtlTrnOutletmanageAssignComponent', () => {
  let component: OtlTrnOutletmanageAssignComponent;
  let fixture: ComponentFixture<OtlTrnOutletmanageAssignComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnOutletmanageAssignComponent]
    });
    fixture = TestBed.createComponent(OtlTrnOutletmanageAssignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
