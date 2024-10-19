import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnOutletmanageUnassignComponent } from './otl-trn-outletmanage-unassign.component';

describe('OtlTrnOutletmanageUnassignComponent', () => {
  let component: OtlTrnOutletmanageUnassignComponent;
  let fixture: ComponentFixture<OtlTrnOutletmanageUnassignComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnOutletmanageUnassignComponent]
    });
    fixture = TestBed.createComponent(OtlTrnOutletmanageUnassignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
