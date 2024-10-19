import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstSalestypeComponent } from './smr-mst-salestype.component';

describe('SmrMstSalestypeComponent', () => {
  let component: SmrMstSalestypeComponent;
  let fixture: ComponentFixture<SmrMstSalestypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstSalestypeComponent]
    });
    fixture = TestBed.createComponent(SmrMstSalestypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
