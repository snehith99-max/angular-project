import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnTcreditnoteaddselectComponent } from './smr-trn-tcreditnoteaddselect.component';

describe('SmrTrnTcreditnoteaddselectComponent', () => {
  let component: SmrTrnTcreditnoteaddselectComponent;
  let fixture: ComponentFixture<SmrTrnTcreditnoteaddselectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnTcreditnoteaddselectComponent]
    });
    fixture = TestBed.createComponent(SmrTrnTcreditnoteaddselectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
