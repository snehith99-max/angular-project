import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnToutletmanagerviewComponent } from './otl-trn-toutletmanagerview.component';

describe('OtlTrnToutletmanagerviewComponent', () => {
  let component: OtlTrnToutletmanagerviewComponent;
  let fixture: ComponentFixture<OtlTrnToutletmanagerviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnToutletmanagerviewComponent]
    });
    fixture = TestBed.createComponent(OtlTrnToutletmanagerviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
