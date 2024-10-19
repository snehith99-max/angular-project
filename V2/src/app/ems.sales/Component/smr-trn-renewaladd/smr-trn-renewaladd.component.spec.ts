import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewaladdComponent } from './smr-trn-renewaladd.component';

describe('SmrTrnRenewaladdComponent', () => {
  let component: SmrTrnRenewaladdComponent;
  let fixture: ComponentFixture<SmrTrnRenewaladdComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewaladdComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewaladdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
