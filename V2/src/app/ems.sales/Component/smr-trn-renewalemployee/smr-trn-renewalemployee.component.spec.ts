import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewalemployeeComponent } from './smr-trn-renewalemployee.component';

describe('SmrTrnRenewalemployeeComponent', () => {
  let component: SmrTrnRenewalemployeeComponent;
  let fixture: ComponentFixture<SmrTrnRenewalemployeeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewalemployeeComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewalemployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
