import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewals360Component } from './smr-trn-renewals360.component';

describe('SmrTrnRenewals360Component', () => {
  let component: SmrTrnRenewals360Component;
  let fixture: ComponentFixture<SmrTrnRenewals360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewals360Component]
    });
    fixture = TestBed.createComponent(SmrTrnRenewals360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
