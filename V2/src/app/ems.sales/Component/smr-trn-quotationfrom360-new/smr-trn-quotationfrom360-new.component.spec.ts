import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnQuotationfrom360NewComponent } from './smr-trn-quotationfrom360-new.component';

describe('SmrTrnQuotationfrom360NewComponent', () => {
  let component: SmrTrnQuotationfrom360NewComponent;
  let fixture: ComponentFixture<SmrTrnQuotationfrom360NewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnQuotationfrom360NewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnQuotationfrom360NewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
