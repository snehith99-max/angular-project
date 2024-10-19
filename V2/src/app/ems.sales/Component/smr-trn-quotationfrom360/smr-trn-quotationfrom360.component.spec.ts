import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnQuotationfrom360Component } from './smr-trn-quotationfrom360.component';

describe('SmrTrnQuotationfrom360Component', () => {
  let component: SmrTrnQuotationfrom360Component;
  let fixture: ComponentFixture<SmrTrnQuotationfrom360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnQuotationfrom360Component]
    });
    fixture = TestBed.createComponent(SmrTrnQuotationfrom360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
