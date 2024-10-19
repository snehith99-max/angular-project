import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewalEditComponent } from './smr-trn-renewal-edit.component';

describe('SmrTrnRenewalEditComponent', () => {
  let component: SmrTrnRenewalEditComponent;
  let fixture: ComponentFixture<SmrTrnRenewalEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewalEditComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewalEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
