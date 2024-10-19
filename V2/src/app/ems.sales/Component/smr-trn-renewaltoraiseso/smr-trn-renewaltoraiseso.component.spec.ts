import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewaltoraisesoComponent } from './smr-trn-renewaltoraiseso.component';

describe('SmrTrnRenewaltoraisesoComponent', () => {
  let component: SmrTrnRenewaltoraisesoComponent;
  let fixture: ComponentFixture<SmrTrnRenewaltoraisesoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewaltoraisesoComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewaltoraisesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
