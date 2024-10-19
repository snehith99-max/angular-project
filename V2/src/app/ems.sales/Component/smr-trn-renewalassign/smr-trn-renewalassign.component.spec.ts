import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewalassignComponent } from './smr-trn-renewalassign.component';

describe('SmrTrnRenewalassignComponent', () => {
  let component: SmrTrnRenewalassignComponent;
  let fixture: ComponentFixture<SmrTrnRenewalassignComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewalassignComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewalassignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
