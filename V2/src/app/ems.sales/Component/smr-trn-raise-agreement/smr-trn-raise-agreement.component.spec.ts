import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRaiseAgreementComponent } from './smr-trn-raise-agreement.component';

describe('SmrTrnRaiseAgreementComponent', () => {
  let component: SmrTrnRaiseAgreementComponent;
  let fixture: ComponentFixture<SmrTrnRaiseAgreementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRaiseAgreementComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRaiseAgreementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
