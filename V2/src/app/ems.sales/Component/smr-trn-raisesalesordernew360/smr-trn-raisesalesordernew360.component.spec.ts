import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRaisesalesordernew360Component } from './smr-trn-raisesalesordernew360.component';

describe('SmrTrnRaisesalesordernew360Component', () => {
  let component: SmrTrnRaisesalesordernew360Component;
  let fixture: ComponentFixture<SmrTrnRaisesalesordernew360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRaisesalesordernew360Component]
    });
    fixture = TestBed.createComponent(SmrTrnRaisesalesordernew360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
