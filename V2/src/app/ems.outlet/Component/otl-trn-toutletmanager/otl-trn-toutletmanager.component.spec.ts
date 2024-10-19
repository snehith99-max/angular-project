import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnToutletmanagerComponent } from './otl-trn-toutletmanager.component';

describe('OtlTrnToutletmanagerComponent', () => {
  let component: OtlTrnToutletmanagerComponent;
  let fixture: ComponentFixture<OtlTrnToutletmanagerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnToutletmanagerComponent]
    });
    fixture = TestBed.createComponent(OtlTrnToutletmanagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
