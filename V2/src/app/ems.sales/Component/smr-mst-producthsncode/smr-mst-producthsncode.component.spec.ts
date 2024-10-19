import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstProducthsncodeComponent } from './smr-mst-producthsncode.component';

describe('SmrMstProducthsncodeComponent', () => {
  let component: SmrMstProducthsncodeComponent;
  let fixture: ComponentFixture<SmrMstProducthsncodeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstProducthsncodeComponent]
    });
    fixture = TestBed.createComponent(SmrMstProducthsncodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
