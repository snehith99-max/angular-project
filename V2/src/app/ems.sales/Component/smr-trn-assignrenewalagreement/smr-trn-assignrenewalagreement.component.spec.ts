import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnAssignrenewalagreementComponent } from './smr-trn-assignrenewalagreement.component';

describe('SmrTrnAssignrenewalagreementComponent', () => {
  let component: SmrTrnAssignrenewalagreementComponent;
  let fixture: ComponentFixture<SmrTrnAssignrenewalagreementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnAssignrenewalagreementComponent]
    });
    fixture = TestBed.createComponent(SmrTrnAssignrenewalagreementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
